using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Character : MonoBehaviour
{
    [Header("Components")]
    ICharacterBrain brain;
    CharacterMotor motor;
    CharacterAnimator animator;
    CharacterCombat combat;
    CharacterStats stats;
    CharacterStates states;
    public CharacterContext Context { get; private set; }
    public CharacterStateMachine StateMachine { get; private set; }

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        brain = GetComponent<ICharacterBrain>();
        animator = GetComponent<CharacterAnimator>();
        combat = GetComponent<CharacterCombat>();

        states = new CharacterStates();

        Context = new CharacterContext(
            this,
            motor,
            animator,
            combat,
            brain,
            stats,
            states);

        StateMachine = new CharacterStateMachine();

        states.Idle = new IdleState(Context, StateMachine);
        states.Move = new MoveState(Context, StateMachine);
        states.Jump = new JumpState(Context, StateMachine);
        states.Fall = new FallState(Context, StateMachine);
        states.Attack = new AttackState(Context, StateMachine);

        StateMachine.ChangeState(states.Idle);
    }

    void Update()
    {
        StateMachine.Update();

        Context.Motor.Tick();
    }

    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
}