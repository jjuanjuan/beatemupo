using UnityEngine;

public class JumpState : CharacterState
{
    public JumpState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Animator.Play("Jump", .05f);

        context.Motor.Jump();
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

            return;
        }

        if (context.Motor.Falling)
        {
            stateMachine.ChangeState(context.States.Fall);
        }

        Vector2 input = context.Brain.MoveInput;

        float speed =
            context.Motor.HorizontalSpeed /
            context.Motor.MoveSpeed;

        context.Animator.SetSpeed(speed);
        context.Motor.Move(input);
    }
}