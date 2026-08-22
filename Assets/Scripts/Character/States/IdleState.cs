using UnityEngine;

public class IdleState : CharacterState
{
    public IdleState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Update()
    {
        context.Motor.Move(Vector3.zero);

        if (context.Brain.PunchPressed)
        {
            context.Combat.StartAttack(context.Combat.Punch);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        if (context.Brain.KickPressed)
        {
            context.Combat.StartAttack(context.Combat.Kick);

            stateMachine.ChangeState(
                context.States.Attack);

            return;
        }

        if (context.Brain.MoveInput.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(context.States.Move);
            return;
        }

        if (context.Brain.JumpPressed && context.Motor.CanJump)
        {
            stateMachine.ChangeState(context.States.Jump);
            return;
        }
    }

    public override void Enter()
    {
        context.Animator.Play("Idle");
    }
}