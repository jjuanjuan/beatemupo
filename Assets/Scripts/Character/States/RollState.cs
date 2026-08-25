using UnityEngine;

public class RollState : CharacterState
{
    private float timer;

    public RollState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        context.Motor.LockMovementInput();

        context.Animator.Play(
            "Roll",
            0.15f);

        context.Motor.TryRoll();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= context.Animator.RollAnimation.duration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.UnlockMovementInput();

        if (context.Motor.Falling)
        {
            stateMachine.ChangeState(
                context.States.Fall);
            return;
        }

        Vector2 input =
            context.Brain.MoveInput;

        if (input.sqrMagnitude > 0.01f)
        {
            stateMachine.ChangeState(
                context.States.Move);
        }
        else
        {
            stateMachine.ChangeState(
                context.States.Idle);
        }
    }

    public override void Exit()
    {
        context.Motor.UnlockMovementInput();
    }
}