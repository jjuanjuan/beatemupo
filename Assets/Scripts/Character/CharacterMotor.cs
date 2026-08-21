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

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f;

    private CharacterController controller;

    private Vector3 desiredVelocity;
    private Vector3 velocity;

    public Vector3 Velocity => velocity;
    public bool Grounded => controller.isGrounded;
    private float lastGroundedTime;
    public bool CanJump => Time.time - lastGroundedTime < coyoteTime;

    public bool Rising => velocity.y > 0f;
    public bool Falling => velocity.y <= 0f;

    public float MoveSpeed => moveSpeed;

    public float HorizontalSpeed =>
        new Vector2(
            velocity.x,
            velocity.z).magnitude;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Tick()
    {
        UpdateHorizontalVelocity();

        ApplyGravity();

        controller.Move(velocity * Time.deltaTime);

        if (Grounded) lastGroundedTime = Time.time;
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
}