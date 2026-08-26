using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private float groundAcceleration = 35f;
    [SerializeField] private float airAcceleration = 12f;

    [SerializeField] private float groundDeceleration = 40f;
    [SerializeField] private float airDeceleration = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -25f;
    [SerializeField] float coyoteTime = .1f;
    [SerializeField] float jumpBufferTime = 0.12f;
    [SerializeField] private float MinLandingDeceleration = 2f;
    [SerializeField] private float MaxLandingDeceleration = 12f;
    [SerializeField] float minFallTime = 0.15f;
    [SerializeField] float maxFallTime = 2.5f;
    [SerializeField] float minLandingDuration = 0.1f;
    [SerializeField] float maxLandingDuration = 0.75f;
    [SerializeField] private float hardFallThreshold = 2f;

    [Header("Wall Jump")]
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private float wallCheckHeight = 1f;
    [SerializeField] private float wallJumpHorizontalForce = 7f;
    [SerializeField] private float wallJumpVerticalForce = 7f;
    [SerializeField] private float wallJumpWindowTime = 2f;
    [SerializeField] private float wallJumpRefreshTime = .5f;
    [SerializeField] private LayerMask wallLayers;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDeceleration = 15f;

    [Header("Ledge")]
    [SerializeField] private float ledgeCheckDistance = 0.7f;
    [SerializeField] private float ledgeCheckHeight = 1.2f;
    [SerializeField] private float ledgeTopHeight = 1.8f;
    [SerializeField] Vector3 ledgeHangOffset = new Vector3(0f, -0.35f, -0.35f);
    [SerializeField] Vector3 ledgeClimbOffset = new Vector3(0f, 0f, 0.2f);
    [SerializeField] private LayerMask ledgeLayers;

    [Header("Aerial Attacks")]
    [SerializeField] private float aerialKickForce = 9f;
    [SerializeField] private float groundPoundForce = 20f;
    [SerializeField] float groundPoundHardFallThreshold = .25f;

    [Header("Roll")]
    [SerializeField]
    private Vector3 rollForce
        = new Vector3(0f, 2f, 10f);
    [SerializeField] private float rollBufferTime = 0.135f;

    private CharacterController controller;

    private Vector3 desiredVelocity;
    private Vector3 velocity;
    private Vector3 knockbackVelocity;

    public Vector3 Velocity => velocity;
    public bool Grounded => controller.isGrounded;
    private float lastGroundedTime;
    public bool CanJump => Time.time - lastGroundedTime < coyoteTime;
    public bool InCoyoteTime => !Grounded &&
        Time.time - lastGroundedTime < coyoteTime;
    private Vector3 attackImpulseVelocity;

    public bool Rising => velocity.y > 0f;
    public bool Falling => velocity.y <= 0f;

    public float MoveSpeed => moveSpeed;

    public float HorizontalSpeed =>
        new Vector2(
            velocity.x,
            velocity.z).magnitude;

    private bool movementLocked;
    public bool MovementLocked => movementLocked;
    private bool movementInputLocked;
    public bool MovementInputLocked => movementInputLocked;

    private bool wallDetected;
    private Vector3 wallNormal;
    public bool WallDetected => wallDetected;
    public Vector3 WallNormal => wallNormal;
    private bool wallJumpAvailable = true;
    public bool WallJumpAvailable => wallJumpAvailable;
    private float wallJumpRefreshTimer;
    private bool wallJumpWindowOpen;
    private float wallJumpWindowTimer;
    public bool WallJumpWindowOpen =>
        wallJumpWindowOpen;

    private bool ledgeDetected;
    private Vector3 ledgeHangPosition;
    private Vector3 ledgeNormal;
    private Vector3 ledgeClimbPosition;
    private bool ledgeHanging;

    public bool LedgeDetected => ledgeDetected;
    public Vector3 LedgeHangPosition => ledgeHangPosition;
    public Vector3 LedgeNormal => ledgeNormal;
    public Vector3 LedgeClimbPosition => ledgeClimbPosition;
    public bool LedgeHanging => ledgeHanging;

    private float fallTime;
    private float lastFallTime;
    private bool landingMovement;

    public float FallTime => fallTime;
    public float LastFallTime => lastFallTime;
    public bool LandingMovement => landingMovement;
    public float MinFallTime => minFallTime;
    public float MaxFallTime => maxFallTime;
    public float MinLandingDuration => minLandingDuration;
    public float MaxLandingDuration => maxLandingDuration;
    public float HardFallThreshold => hardFallThreshold;

    private bool aerialAttackUsed;

    public bool AerialAttackUsed => aerialAttackUsed;
    public float GroundPoundHardFallThreshold => groundPoundHardFallThreshold;

    private float jumpBufferTimer;
    private float rollBufferTimer;

    public bool JumpBuffered => jumpBufferTimer > 0f;
    public bool RollBuffered => rollBufferTimer > 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Tick()
    {
        UpdateJumpBuffer();
        UpdateRollBuffer();

        UpdateHorizontalVelocity();

        if (!ledgeHanging)
            ApplyGravity();

        CheckWall();
        CheckLedge();

        Vector3 finalVelocity =
            velocity +
            knockbackVelocity +
            attackImpulseVelocity;

        controller.Move(
            finalVelocity * Time.deltaTime);

        UpdateKnockback();
        UpdateWallJumpRefresh();
        UpdateWallJumpWindow();

        if (Grounded)
        {
            lastGroundedTime = Time.time;
            ResetAerialAttack();
        }
    }

    public void StartFall()
    {
        fallTime = 0f;
    }

    public void UpdateFallTime()
    {
        fallTime += Time.deltaTime;
    }

    public void EndFall()
    {
        lastFallTime = fallTime;
        Debug.Log("last fall time: " + lastFallTime);
    }

    public void StartLandingMovement()
    {
        landingMovement = true;
    }

    public void EndLandingMovement()
    {
        landingMovement = false;
    }

    private void UpdateWallJumpRefresh()
    {
        if (wallJumpAvailable)
            return;

        if (Grounded)
        {
            wallJumpAvailable = true;
            return;
        }

        wallJumpRefreshTimer += Time.deltaTime;

        if (wallJumpRefreshTimer >= wallJumpRefreshTime)
        {
            wallJumpAvailable = true;
            wallJumpRefreshTimer = 0f;
        }
    }

    private void UpdateKnockback()
    {
        knockbackVelocity.x = Mathf.MoveTowards(
            knockbackVelocity.x,
            0f,
            knockbackDeceleration * Time.deltaTime);

        knockbackVelocity.z = Mathf.MoveTowards(
            knockbackVelocity.z,
            0f,
            knockbackDeceleration * Time.deltaTime);

        if (knockbackVelocity.y != 0f)
        {
            knockbackVelocity.y +=
                gravity * Time.deltaTime;

            if (Grounded && knockbackVelocity.y < 0f)
            {
                knockbackVelocity.y = 0f;
            }
        }
    }

    public void Move(Vector2 input)
    {
        if (cameraTransform == null)
            return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignorar la inclinación de la cámara
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * input.y + right * input.x;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        desiredVelocity = direction * moveSpeed;

        RotateTowards(direction, false);
    }
    public void Jump()
    {
        if (!CanJump)
            return;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void RotateTowards(Vector3 direction, bool instant)
    {
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (instant)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateHorizontalVelocity()
    {
        if (movementLocked)
        {
            velocity.x = Mathf.MoveTowards(
                velocity.x,
                0f,
                groundDeceleration * Time.deltaTime);

            velocity.z = Mathf.MoveTowards(
                velocity.z,
                0f,
                groundDeceleration * Time.deltaTime);

            return;
        }

        if (landingMovement)
        {
            float fallIntensity =
                Mathf.InverseLerp(
                    minFallTime,
                    maxFallTime,
                    lastFallTime);

            float landingDeceleration =
                Mathf.Lerp(
                    MinLandingDeceleration,
                    MaxLandingDeceleration,
                    fallIntensity);

            velocity.x = Mathf.MoveTowards(
                velocity.x,
                0f,
                landingDeceleration * Time.deltaTime);

            velocity.z = Mathf.MoveTowards(
                velocity.z,
                0f,
                landingDeceleration * Time.deltaTime);

            return;
        }

        bool moving = desiredVelocity.sqrMagnitude > 0.01f;

        float accel;

        if (Grounded)
            accel = moving ? groundAcceleration : groundDeceleration;
        else
            accel = moving ? airAcceleration : airDeceleration;

        velocity.x = Mathf.MoveTowards(
            velocity.x,
            desiredVelocity.x,
            accel * Time.deltaTime);

        velocity.z = Mathf.MoveTowards(
            velocity.z,
            desiredVelocity.z,
            accel * Time.deltaTime);
    }

    public void SetLedgeClimbPosition(
        Vector3 startPosition,
        Vector3 targetPosition,
        Vector3 ledgeRight,
        Vector3 ledgeForward,
        float progressX,
        float progressY,
        float progressZ)
    {
        Vector3 totalMovement =
            targetPosition - startPosition;

        float lateralMovement =
            Vector3.Dot(
                totalMovement,
                ledgeRight);

        float verticalMovement =
            totalMovement.y;

        float forwardMovement =
            Vector3.Dot(
                totalMovement,
                ledgeForward);

        Vector3 position =
            startPosition
            + ledgeRight *
              (lateralMovement * progressX)
            + Vector3.up *
              (verticalMovement * progressY)
            + ledgeForward *
              (forwardMovement * progressZ);

        SetPosition(position);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void ApplyGravity()
    {
        if (Grounded)
        {
            if (velocity.y < 0f)
                velocity.y = -2f;

            return;
        }

        if (InCoyoteTime)
            return;

        velocity.y += gravity * Time.deltaTime;
    }

    public void LockMovement()
    {
        movementLocked = true;

        desiredVelocity.x = 0f;
        desiredVelocity.z = 0f;
    }

    public void UnlockMovement()
    {
        movementLocked = false;
    }

    public void LockMovementInput()
    {
        movementInputLocked = true;

        desiredVelocity.x = 0f;
        desiredVelocity.z = 0f;
    }

    public void UnlockMovementInput()
    {
        movementInputLocked = false;
    }

    public void AddImpulse(Vector3 impulse)
    {
        velocity += impulse;
    }

    public void ApplyKnockback(
        Vector3 direction,
        float horizontalForce,
        float verticalForce)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
            direction.Normalize();

        knockbackVelocity.x =
            direction.x * horizontalForce;

        knockbackVelocity.z =
            direction.z * horizontalForce;

        knockbackVelocity.y =
            verticalForce;

        Debug.Log(
            $"KNOCKBACK | " +
            $"Direction: {direction} | " +
            $"Horizontal: {horizontalForce} | " +
            $"Vertical: {verticalForce}");
    }

    public void StopHorizontalMovement()
    {
        velocity.x = 0f;
        velocity.z = 0f;

        desiredVelocity.x = 0f;
        desiredVelocity.z = 0f;
    }
    public void StopVerticalMovement()
    {
        velocity.y = 0f;
        knockbackVelocity.y = 0f;
    }

    public void StartAttackMovement(float speed)
    {
        if (speed <= 0f)
            return;

        Vector3 direction = transform.forward;

        attackImpulseVelocity =
            direction * speed;
    }
    public void StopAttackMovement()
    {
        attackImpulseVelocity = Vector3.zero;
    }

    public void EndAttack()
    {
        movementInputLocked = false;
        attackImpulseVelocity = Vector3.zero;
    }

    public void FaceTarget(
        Character target, bool instant)
    {
        if (target == null)
            return;

        FacePosition(
            target.transform.position, instant);
    }

    public void FacePosition(Vector3 position,
        bool instant)
    {
        Vector3 direction =
            position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        RotateTowards(direction, instant);
    }

    public void DisableCharacterController()
    {
        controller.enabled = false;
    }
    public void EnableCharacterController()
    {
        controller.enabled = true;
    }

    public void CheckWall()
    {
        bool wasWallDetected = wallDetected;

        wallDetected = false;

        Vector3 origin =
            transform.position +
            Vector3.up * wallCheckHeight;

        Vector3[] directions =
        {
        transform.forward,
        -transform.forward,
        transform.right,
        -transform.right
    };

        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                wallCheckDistance,
                wallLayers))
            {
                if (hit.collider.transform == transform)
                    continue;

                wallDetected = true;
                wallNormal = hit.normal;

                break;
            }
        }

        if (Grounded)
        {
            wallJumpWindowOpen = false;
            wallJumpWindowTimer = 0f;
            return;
        }

        if (wallDetected && !wasWallDetected)
        {
            wallJumpWindowOpen = true;
            wallJumpWindowTimer = 0f;
        }
    }

    private void UpdateWallJumpWindow()
    {
        if (!wallJumpWindowOpen)
            return;

        if (Grounded)
        {
            wallJumpWindowOpen = false;
            wallJumpWindowTimer = 0f;
            return;
        }

        wallJumpWindowTimer += Time.deltaTime;

        if (wallJumpWindowTimer >= wallJumpWindowTime)
        {
            wallJumpWindowOpen = false;
            wallJumpWindowTimer = 0f;
        }
    }

    public void WallJump()
    {
        if (!wallJumpWindowOpen ||
            !wallJumpAvailable)
            return;

        Vector3 direction = wallNormal;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        velocity.x =
            direction.x * wallJumpHorizontalForce;

        velocity.z =
            direction.z * wallJumpHorizontalForce;

        velocity.y =
            wallJumpVerticalForce;

        desiredVelocity = Vector3.zero;

        wallJumpAvailable = false;
        wallJumpRefreshTimer = 0f;

        wallJumpWindowOpen = false;
        wallJumpWindowTimer = 0f;

        RotateTowards(direction, false);

        ResetAerialAttack();
    }

    public void CheckLedge()
    {
        ledgeDetected = false;

        if (Grounded)
            return;

        Vector3 lowerOrigin =
            transform.position +
            Vector3.up * ledgeCheckHeight;

        Vector3 upperOrigin =
            transform.position +
            Vector3.up * ledgeTopHeight;

        // Tiene que existir una pared delante del personaje.
        if (!Physics.Raycast(
            lowerOrigin,
            transform.forward,
            out RaycastHit wallHit,
            ledgeCheckDistance,
            ledgeLayers))
        {
            return;
        }

        // Por encima de la pared tiene que existir espacio libre.
        if (Physics.Raycast(
            upperOrigin,
            transform.forward,
            ledgeCheckDistance,
            ledgeLayers))
        {
            return;
        }

        // Buscamos la superficie superior del ledge.
        Vector3 topRayOrigin =
            wallHit.point +
            Vector3.up * 0.05f;

        if (!Physics.Raycast(
            topRayOrigin,
            Vector3.down,
            out RaycastHit topHit,
            ledgeTopHeight,
            ledgeLayers))
        {
            return;
        }

        ledgeNormal = wallHit.normal;

        ledgeHangPosition =
            topHit.point +
            transform.right * ledgeHangOffset.x +
            Vector3.up * ledgeHangOffset.y +
            transform.forward * ledgeHangOffset.z;

        ledgeClimbPosition =
            topHit.point
            + transform.right * ledgeClimbOffset.x
            + Vector3.up * ledgeClimbOffset.y
            + transform.forward * ledgeClimbOffset.z;

        ledgeDetected = true;
    }

    public void StartLedgeHang()
    {
        ledgeHanging = true;

        LockMovement();

        velocity = Vector3.zero;
        knockbackVelocity = Vector3.zero;
        attackImpulseVelocity = Vector3.zero;

        desiredVelocity = Vector3.zero;

        Vector3 facingDirection = -ledgeNormal;
        facingDirection.y = 0f;

        if (facingDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(facingDirection);
        }
    }

    public void EndLedgeHang()
    {
        EnableCharacterController();
        ledgeHanging = false;
        UnlockMovementInput();
        UnlockMovement();
    }

    public void SetLedgeHangPosition()
    {
        DisableCharacterController();

        transform.position =
            ledgeHangPosition;
    }

    public void SetLedgeClimbPosition()
    {
        DisableCharacterController();

        transform.position =
            ledgeClimbPosition;

        EnableCharacterController();
    }

    public Vector3 InputToWorldDirection(Vector2 input)
    {
        if (cameraTransform == null)
            return Vector3.zero;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction =
            forward * input.y +
            right * input.x;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        return direction;
    }

    private void OnControllerColliderHit(
        ControllerColliderHit hit)
    {
        if (velocity.y <= 0f)
            return;

        // si te pegás en la cabeza saltando, dejá de ascender
        if (hit.normal.y < -0.5f)
        {
            velocity.y = 0f;
        }
    }

    public void ConsumeAerialAttack()
    {
        aerialAttackUsed = true;
    }

    public void ResetAerialAttack()
    {
        aerialAttackUsed = false;
    }

    public void AerialKick()
    {
        velocity.y = aerialKickForce;
    }

    public void GroundPound()
    {
        velocity.x = 0f;
        velocity.z = 0f;

        desiredVelocity.x = 0f;
        desiredVelocity.z = 0f;

        knockbackVelocity.x = 0f;
        knockbackVelocity.z = 0f;

        attackImpulseVelocity = Vector3.zero;

        velocity.y = -groundPoundForce;
    }

    public void BufferJump()
    {
        jumpBufferTimer = jumpBufferTime;
    }

    private void UpdateJumpBuffer()
    {
        if (jumpBufferTimer <= 0f)
            return;

        jumpBufferTimer -= Time.deltaTime;
    }

    public void BufferRoll()
    {
        rollBufferTimer = rollBufferTime;
    }

    private void UpdateRollBuffer()
    {
        if (rollBufferTimer <= 0f)
            return;

        rollBufferTimer -= Time.deltaTime;
    }

    public bool TryRoll()
    {
        rollBufferTimer = 0f;

        Vector3 impulse =
            transform.forward * rollForce.z;

        velocity += impulse;
        velocity = new Vector3(velocity.x, rollForce.y, velocity.z);

        desiredVelocity.x = 0f;
        desiredVelocity.z = 0f;

        return true;
    }

    /// GIZMOS /////////////////////////////////////////
    private void OnDrawGizmosSelected()
    {
        DrawWallCheckGizmos();
        DrawLedgeCheckGizmos();
    }

    private void DrawWallCheckGizmos()
    {
        Vector3 origin =
            transform.position +
            Vector3.up * wallCheckHeight;

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(
            origin,
            origin + transform.forward * wallCheckDistance);

        Gizmos.DrawLine(
            origin,
            origin - transform.forward * wallCheckDistance);

        Gizmos.DrawLine(
            origin,
            origin + transform.right * wallCheckDistance);

        Gizmos.DrawLine(
            origin,
            origin - transform.right * wallCheckDistance);
    }

    private void DrawLedgeCheckGizmos()
    {
        Vector3 lowerOrigin =
            transform.position +
            Vector3.up * ledgeCheckHeight;

        Vector3 upperOrigin =
            transform.position +
            Vector3.up * ledgeTopHeight;

        // ------------------------------------------------
        // 1. Wall check
        // ------------------------------------------------

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            lowerOrigin,
            lowerOrigin +
            transform.forward * ledgeCheckDistance);

        Gizmos.DrawWireSphere(
            lowerOrigin +
            transform.forward * ledgeCheckDistance,
            0.04f);

        // ------------------------------------------------
        // 2. Upper clearance check
        // ------------------------------------------------

        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(
            upperOrigin,
            upperOrigin +
            transform.forward * ledgeCheckDistance);

        Gizmos.DrawWireSphere(
            upperOrigin +
            transform.forward * ledgeCheckDistance,
            0.04f);

        // ------------------------------------------------
        // 4. Posición de hang
        // ------------------------------------------------

        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(
            ledgeHangPosition,
            0.12f);

        // ------------------------------------------------
        // 5. Posición de climb
        // ------------------------------------------------

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            ledgeClimbPosition,
            0.12f);

        // Línea entre hang y climb.
        Gizmos.DrawLine(
            ledgeHangPosition,
            ledgeClimbPosition);
    }
}