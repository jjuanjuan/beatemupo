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
        if (!context.Motor.Grounded &&
             !context.Motor.CanJump)
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

        if (context.Brain.MoveDirection.sqrMagnitude < 0.01f)
        {
            context.Animator.SetSpeed(0f);
            stateMachine.ChangeState(
                context.States.Idle);

            return;
        }

        float speed =
            context.Motor.HorizontalSpeed /
            context.Motor.MoveSpeed;

        context.Animator.SetSpeed(speed);
        context.Motor.MoveWorldDirection(
            context.Brain.MoveDirection);
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