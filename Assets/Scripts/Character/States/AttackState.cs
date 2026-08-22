using UnityEngine;

public class AttackState : CharacterState
{
    private float timer;

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

        context.Animator.Play(
            attack.animationState,
            0.05f);

        context.Motor.StartAttack();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        AttackDefinition attack =
            context.Combat.CurrentAttack;

        if (timer >= attack.duration)
        {
            context.Combat.EndAttack();

            context.Motor.EndAttack();

            Vector2 input =
                context.Brain.MoveInput;

            if (input.sqrMagnitude > 0.01f)
            {
                stateMachine.ChangeState(
                    context.States.Move);
            }
            else
            {
                stateMachine.ChangeState(
                    context.States.Idle);
            }
        }
    }
}