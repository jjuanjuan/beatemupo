using UnityEngine;

public class JumpState : CharacterState
{
    public JumpState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        context.Motor.Jump();
    }

    public override void Update()
    {
        context.Motor.Move(context.Brain.MoveInput);

        if (context.Motor.Falling)
            stateMachine.ChangeState(context.States.Fall);
    }
}