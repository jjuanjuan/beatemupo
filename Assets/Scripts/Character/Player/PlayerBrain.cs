using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBrain : MonoBehaviour, ICharacterBrain
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool PunchPressed { get; private set; }
    public bool KickPressed { get; private set; }
    public bool RollPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool ProjectionPressed { get; private set; }

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
        MoveInput = input.Player.Movement.ReadValue<Vector2>();
        LookInput = input.Player.Look.ReadValue<Vector2>();

        JumpPressed = input.Player.Jump.WasPressedThisFrame();

        PunchPressed = input.Player.Punch.WasPressedThisFrame();
        KickPressed = input.Player.Kick.WasPressedThisFrame();

        RollPressed = input.Player.Roll.WasPressedThisFrame();
        
        InteractPressed = input.Player.Interact.WasPressedThisFrame();
        
        ProjectionPressed = input.Player.Interact.WasPressedThisFrame();

        if (input.Debug.Reset.WasPressedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}