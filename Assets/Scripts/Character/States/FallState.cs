using UnityEngine;

public class FallState : CharacterState
{
    public FallState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Animator.Play("Fall", .05f);
    }

    public override void Update()
    {
        if (context.Motor.LedgeDetected)
        {
            stateMachine.ChangeState(
                context.States.LedgeHang);

            return;
        }

        if (context.Motor.WallJumpWindowOpen &&
            context.Brain.JumpPressed)
        {
            context.Motor.WallJump();

            context.Animator.Play(
                "WallJump",
                0.05f);

            stateMachine.ChangeState(
                context.States.Jump);

            return;
        }

        if (context.Motor.Grounded)
        {
            if (context.Brain.MoveInput.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(context.States.Move);
            else
                stateMachine.ChangeState(context.States.Idle);
        }
    }
}