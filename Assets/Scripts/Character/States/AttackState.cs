using UnityEngine;

public class AttackState : CharacterState
{
    private float timer;

    public AttackPhase Phase { get; private set; }

    public AttackState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        AttackDefinition attack =
            context.Combat.CurrentAttack;

        Phase = AttackPhase.Startup;

        context.Animator.Play(
            attack.animationState,
            0.05f);

        context.Motor.StartAttack();
    }

    public override void Update()
    {
        AttackDefinition attack =
            context.Combat.CurrentAttack;

        timer += Time.deltaTime;

        UpdatePhase(attack);

        if (timer >= attack.duration)
        {
            FinishAttack();
        }
    }

    private void UpdatePhase(AttackDefinition attack)
    {
        AttackPhase previousPhase = Phase;

        if (timer < attack.hitStart)
            Phase = AttackPhase.Startup;
        else if (timer < attack.hitEnd)
            Phase = AttackPhase.Active;
        else
            Phase = AttackPhase.Recovery;

        if (previousPhase == Phase)
            return;

        switch (Phase)
        {
            case AttackPhase.Startup:
                break;

            case AttackPhase.Active:
                context.Combat.BeginHitbox();
                break;

            case AttackPhase.Recovery:
                context.Combat.EndHitbox();
                break;
        }
    }

    private void FinishAttack()
    {
        context.Combat.EndHitbox();
        context.Combat.EndAttack();
        context.Motor.EndAttack();

        Vector2 input =
            context.Brain.MoveInput;

        if (input.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(context.States.Move);
        else
            stateMachine.ChangeState(context.States.Idle);
    }

    public override void Exit()
    {
        context.Motor.EndAttack();
    }
}

public enum AttackPhase
{
    Startup,
    Active,
    Recovery
}