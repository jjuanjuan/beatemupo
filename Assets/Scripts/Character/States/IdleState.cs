using UnityEngine;

public class IdleState : CharacterState
{
    public IdleState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Update()
    {
        if (!context.Motor.Grounded &&
             !context.Motor.CanJump)
        {
            stateMachine.ChangeState(
                context.States.Fall);

            return;
        }

        context.Motor.Move(Vector3.zero);

        if (context.Brain.ProjectionPressed)
        {
            context.Projection.Toggle();
        }

        if (context.Brain.PunchPressed)
        {
            FaceAttackTarget();

            context.Combat.ResetCombo();

            context.Combat.StartAttack(
                context.Combat.Punch);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        if (context.Brain.KickPressed)
        {
            FaceAttackTarget();

            context.Combat.ResetCombo();

            context.Combat.StartAttack(
                context.Combat.Kick);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        if (context.Brain.MoveInput.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(
                context.States.Move);

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
    }

    public override void Enter()
    {
        context.Animator.Play("Idle");
    }

    private void FaceAttackTarget()
    {
        Character target =
            context.Targeting.FindClosestCharacter();

        if (target != null)
        {
            context.Motor.FaceTarget(target, true);
        }
    }
}