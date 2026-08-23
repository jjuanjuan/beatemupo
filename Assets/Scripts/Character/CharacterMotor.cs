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

    [Header("Wall Jump")]
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private float wallCheckHeight = 1f;
    [SerializeField] private float wallJumpHorizontalForce = 7f;
    [SerializeField] private float wallJumpVerticalForce = 7f;
    [SerializeField] private float wallJumpApexWindow = 0.15f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDeceleration = 15f;

    private CharacterController controller;

    private Vector3 desiredVelocity;
    private Vector3 velocity;
    private Vector3 knockbackVelocity;

    public Vector3 Velocity => velocity;
    public bool Grounded => controller.isGrounded;
    private float lastGroundedTime;
    public bool CanJump => Time.time - lastGroundedTime < coyoteTime;
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Tick()
    {
        UpdateHorizontalVelocity();
        ApplyGravity();
        CheckWall();

        Vector3 finalVelocity =
            velocity +
            knockbackVelocity +
            attackImpulseVelocity;

        controller.Move(
            finalVelocity * Time.deltaTime);

        UpdateKnockback();

        if (Grounded)
            lastGroundedTime = Time.time;
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

        RotateTowards(direction);
    }
    public void Jump()
    {
        if (!CanJump)
            return;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public void RotateTowards(Vector3 direction)
    {
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
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

    private void ApplyGravity()
    {
        if (Grounded && velocity.y < 0f)
            velocity.y = -2f;

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

    public void FacePosition(Vector3 position)
    {
        Vector3 direction = position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation =
            Quaternion.LookRotation(direction);
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
                wallCheckDistance))
            {
                if (hit.collider.transform == transform)
                    continue;

                wallDetected = true;
                wallNormal = hit.normal;

                return;
            }
        }
    }
}