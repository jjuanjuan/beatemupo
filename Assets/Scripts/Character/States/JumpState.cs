using UnityEngine;

public class JumpState : CharacterState
{
    Vector3 jumpDirection;

    public JumpState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        jumpDirection =
            context.Brain.JumpDirection;

        jumpDirection.y = 0f;

        if (jumpDirection.sqrMagnitude > 0.01f)
        {
            jumpDirection.Normalize();

            context.Motor.RotateTowards(
                jumpDirection,
                true);
        }

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

        if (context.Brain.JumpPressed)
        {
            context.Motor.BufferJump();
        }

        if (context.Motor.WallJumpWindowOpen &&
            context.Motor.JumpBuffered)
        {
            context.Motor.WallJump();

            context.Animator.Play(
                "WallJump",
                0.05f);

            stateMachine.ChangeState(
                context.States.Jump);

            return;
        }

        if (context.Brain.JumpPressed &&
            context.Motor.CanJump)
        {
            stateMachine.ChangeState(
                context.States.Jump);

            return;
        }

        if (context.Brain.RollPressed)
        {
            context.Motor.BufferRoll();
        }

        if (context.Motor.RollBuffered &&
            context.Motor.Grounded)
        {
            stateMachine.ChangeState(
                context.States.Roll);

            return;
        }

        if (!context.Motor.AerialAttackUsed &&
            context.Brain.KickPressed)
        {
            stateMachine.ChangeState(
                context.States.AerialKick);

            return;
        }

        if (!context.Motor.AerialAttackUsed &&
            context.Brain.PunchPressed)
        {
            stateMachine.ChangeState(
                context.States.GroundPound);

            return;
        }

        if (context.Motor.Falling)
        {
            stateMachine.ChangeState(
                context.States.Fall);

            return;
        }

        float speed =
            context.Motor.HorizontalSpeed /
            context.Motor.MoveSpeed;

        context.Animator.SetSpeed(speed);

        context.Motor.MoveWorldDirection(
            context.Brain.MoveDirection);
    }
}