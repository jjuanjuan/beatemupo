using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBrain : MonoBehaviour, ICharacterBrain
{
    [SerializeField]
    private Transform cameraTransform;

    public Vector2 MoveInput { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool PunchPressed { get; private set; }
    public bool KickPressed { get; private set; }
    public bool RollPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    private PlayerInput input;

    void Awake()
    {
        input = new PlayerInput();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        MoveInput =
            input.Player.Movement
                .ReadValue<Vector2>();

        UpdateMoveDirection();

        LookInput = input.Player.Look.ReadValue<Vector2>();

        JumpPressed = input.Player.Jump.WasPressedThisFrame();

        PunchPressed = input.Player.Punch.WasPressedThisFrame();
        KickPressed = input.Player.Kick.WasPressedThisFrame();

        RollPressed = input.Player.Roll.WasPressedThisFrame();

        InteractPressed = input.Player.Interact.WasPressedThisFrame();

        if (input.Debug.Reset.WasPressedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (input.Debug.SpawnEnemy.WasPressedThisFrame())
        {
            var spawner = FindAnyObjectByType<EnemySpawner>();
            spawner.Spawn();
        }
    }

    private void UpdateMoveDirection()
    {
        if (cameraTransform == null)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        Vector3 forward =
            cameraTransform.forward;

        Vector3 right =
            cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        MoveDirection =
            forward * MoveInput.y +
            right * MoveInput.x;

        if (MoveDirection.sqrMagnitude > 1f)
        {
            MoveDirection.Normalize();
        }
    }
}