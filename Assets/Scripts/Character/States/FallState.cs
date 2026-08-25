using UnityEngine;

public class FallState : CharacterState
{
    public FallState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Motor.StartFall();

        context.Animator.Play(
            "Fall",
            0.05f);
    }

    public override void Update()
    {
        context.Motor.UpdateFallTime();

        context.Animator.SetFallTime(
            context.Motor.FallTime);

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

        if (context.Motor.Grounded)
        {
            context.Motor.EndFall();

            if (context.Motor.LastFallTime >=
                context.Motor.HardFallThreshold)
            {
                stateMachine.ChangeState(
                    context.States.Splat);

                return;
            }

            stateMachine.ChangeState(
                context.States.Landing);

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

        Vector2 input = context.Brain.MoveInput;

        float speed =
            context.Motor.HorizontalSpeed /
            context.Motor.MoveSpeed;

        context.Animator.SetSpeed(speed);
        context.Motor.Move(input);
    }
}