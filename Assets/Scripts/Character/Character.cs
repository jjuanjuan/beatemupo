using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Character : MonoBehaviour
{
    [Header("Components")]
    ICharacterBrain brain;
    CharacterMotor motor;
    CharacterAnimator animator;
    CharacterCombat combat;
    CharacterDamage damage;
    CharacterStats stats;
    CharacterStates states;
    CharacterTargeting targeting;
    public CharacterContext Context { get; private set; }
    public CharacterStateMachine StateMachine { get; private set; }

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        brain = GetComponent<ICharacterBrain>();
        animator = GetComponent<CharacterAnimator>();
        combat = GetComponent<CharacterCombat>();
        damage = GetComponent<CharacterDamage>();
        targeting = GetComponent<CharacterTargeting>();

        states = new CharacterStates();

        StateMachine = new CharacterStateMachine();

        Context = new CharacterContext(
            this,
            motor,
            animator,
            combat,
            damage,
            brain,
            targeting,
            stats,
            states);

        if (damage != null)
        {
            damage.Initialize(Context);
        }

        states.Idle = new IdleState(Context, StateMachine);
        states.Move = new MoveState(Context, StateMachine);
        states.Jump = new JumpState(Context, StateMachine);
        states.Fall = new FallState(Context, StateMachine);
        states.Attack = new AttackState(Context, StateMachine);
        states.Hit = new HitState(Context, StateMachine);
        states.Knockdown = new KnockdownState(Context, StateMachine);
        states.Death = new DeathState(Context, StateMachine);
        states.LedgeHang = new LedgeHangState(Context, StateMachine);
        states.LedgeClimb = new LedgeClimbState(Context, StateMachine);

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