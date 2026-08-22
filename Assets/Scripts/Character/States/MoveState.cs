using UnityEngine;

public class MoveState : CharacterState
{
    public MoveState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Animator.Play("Move");
    }

    public override void Update()
    {
        if (!context.Motor.Grounded)
        {
            stateMachine.ChangeState(
                context.States.Fall);

            return;
        }

        if (context.Brain.JumpPressed &&
            context.Motor.CanJump)
        {
            stateMachine.ChangeState(
                context.States.Jump);

            return;
        }

        if (context.Brain.PunchPressed)
        {
            context.Combat.ResetCombo();

            context.Combat.StartAttack(
                context.Combat.Punch);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        if (context.Brain.KickPressed)
        {
            context.Combat.ResetCombo();

            context.Combat.StartAttack(
                context.Combat.Kick);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        Vector2 input =
            context.Brain.MoveInput;

        if (input.sqrMagnitude < 0.01f)
        {
            stateMachine.ChangeState(
                context.States.Idle);

            return;
        }

        float speed =
            context.Motor.HorizontalSpeed /
            context.Motor.MoveSpeed;

        context.Animator.SetSpeed(speed);
        context.Motor.Move(input);
    }
}