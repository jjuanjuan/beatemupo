using UnityEngine;

public class PlayerBrain : MonoBehaviour, ICharacterBrain
{
    public Vector2 MoveInput { get; private set; }

    public bool JumpPressed { get; private set; }

    public bool AttackPressed { get; private set; }

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

        JumpPressed = input.Player.Jump.WasPressedThisFrame();

        //AttackPressed = input.Player.Attack.WasPressedThisFrame();
    }
}