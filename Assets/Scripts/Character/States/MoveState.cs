using UnityEngine;

public class MoveState : CharacterState
{
    public MoveState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Update()
    {
        if (!context.Motor.Grounded)
        {
            stateMachine.ChangeState(context.States.Fall);
            return;
        }

        if (context.Brain.JumpPressed && context.Motor.CanJump)
        {
            stateMachine.ChangeState(context.States.Jump);
            return;
        }

        Vector2 input = context.Brain.MoveInput;

        if (input.sqrMagnitude < 0.01f)
        {
            stateMachine.ChangeState(context.States.Idle);
            return;
        }

        context.Motor.Move(input);
    }
}