using UnityEngine;

public class FallState : CharacterState
{
    public FallState(CharacterContext context, CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Update()
    {
        context.Motor.Move(context.Brain.MoveInput);

        if (context.Motor.Grounded)
        {
            if (context.Brain.MoveInput.sqrMagnitude > 0.01f)
                stateMachine.ChangeState(context.States.Move);
            else
                stateMachine.ChangeState(context.States.Idle);
        }
    }
}