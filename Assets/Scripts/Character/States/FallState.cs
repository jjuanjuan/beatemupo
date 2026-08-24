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
    }
}