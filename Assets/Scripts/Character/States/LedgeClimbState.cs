using UnityEngine;

public class LedgeClimbState : CharacterState
{
    private float timer;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    public LedgeClimbState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        startPosition =
            context.Motor.LedgeHangPosition;

        targetPosition =
            context.Motor.LedgeClimbPosition;

        context.Motor.LockMovement();
        context.Motor.StopHorizontalMovement();

        context.Animator.Play(
            context.Animator.ledgeClimbAnimation.animationState,
            0.05f);
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        float progress =
            Mathf.Clamp01(
                timer / context.Animator.ledgeClimbAnimation.duration);

        float progressX =
            context.Animator.ledgeClimbAnimation.movementX.Evaluate(progress);

        float progressY =
            context.Animator.ledgeClimbAnimation.movementY.Evaluate(progress);

        float progressZ =
            context.Animator.ledgeClimbAnimation.movementZ.Evaluate(progress);

        Vector3 ledgeForward =
            -context.Motor.LedgeNormal;

        ledgeForward.y = 0f;
        ledgeForward.Normalize();

        Vector3 ledgeRight =
            Vector3.Cross(
                Vector3.up,
                ledgeForward).normalized;

        context.Motor.SetLedgeClimbPosition(
            startPosition,
            targetPosition,
            ledgeRight,
            ledgeForward,
            progressX,
            progressY,
            progressZ);

        if (progress >= 1f)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.SetLedgeClimbPosition();

        context.Motor.EnableCharacterController();

        stateMachine.ChangeState(
            context.States.Idle);
    }

    public override void Exit()
    {
        context.Motor.EndLedgeHang();
    }
}