using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterMotor motor;
    [SerializeField] private CharacterAnimator animator;
    [SerializeField] private CharacterCombat combat;
    [SerializeField] private ICharacterBrain brain;
    [SerializeField] private CharacterStats stats;

    public CharacterContext Context { get; private set; }

    public CharacterStateMachine StateMachine { get; private set; }

    void Awake()
    {
        brain = GetComponent<ICharacterBrain>();

        if (brain == null)
        {
            Debug.LogError($"{name} has no ICharacterBrain.");
            return;
        }

        motor = GetComponent<CharacterMotor>();
        combat = GetComponent<CharacterCombat>();
        animator = GetComponent<CharacterAnimator>();

        Context = new CharacterContext(
            this,
            motor,
            animator,
            combat,
            brain,
            stats);

        StateMachine = new CharacterStateMachine();
    }

    void Update()
    {
        StateMachine.Update();

        motor.Tick();
    }

    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
}