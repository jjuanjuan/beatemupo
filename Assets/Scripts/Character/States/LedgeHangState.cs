using UnityEngine;

public class LedgeHangState : CharacterState
{
    public LedgeHangState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Motor.StartLedgeHang();

        context.Motor.SetLedgeHangPosition();

        context.Animator.Play(
            "LedgeHang",
            0.05f);

        context.Projection.Deactivate();
    }

    public override void Update()
    {
        Vector2 input = context.Brain.MoveInput;

        if (context.Brain.JumpPressed)
        {
            stateMachine.ChangeState(
                context.States.LedgeClimb);

            return;
        }

        if (ShouldDrop(input))
        {
            Drop();

            return;
        }
    }

    private bool ShouldDrop(Vector2 input)
    {
        if (input.sqrMagnitude < 0.25f)
            return false;

        Vector3 ledgeNormal =
            context.Motor.LedgeNormal;

        Vector3 inputDirection =
            context.Motor.InputToWorldDirection(input);

        // El jugador quiere alejarse de la pared.
        float awayFromLedge =
            Vector3.Dot(
                inputDirection,
                ledgeNormal);

        return awayFromLedge > 0.5f;
    }

    private void Drop()
    {
        context.Motor.EndLedgeHang();

        stateMachine.ChangeState(
            context.States.Fall);
    }

    public override void Exit()
    {
        context.Motor.UnlockMovement();
    }
}