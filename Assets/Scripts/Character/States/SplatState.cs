using UnityEngine;

public class SplatState : CharacterState
{
    private enum Phase
    {
        Splat,
        Down,
        GettingUp
    }

    private Phase phase;
    private float timer;

    private AnimationDefinition splatAnimation;
    private AnimationDefinition downAnimation;
    private AnimationDefinition getUpAnimation;

    public SplatState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;
        phase = Phase.Splat;

        context.Motor.LockMovementInput();
        context.Motor.StopHorizontalMovement();

        splatAnimation = context.Animator.SplatAnimation;
        downAnimation = context.Animator.SplatDownAnimation;
        getUpAnimation = context.Animator.SplatGetUpAnimation;


        context.Animator.Play(
            splatAnimation.animationState,
            0.08f);

        context.Projection.Deactivate();
    }

    public override void Update()
    {
        switch (phase)
        {
            case Phase.Splat:
                UpdateSplat();
                break;

            case Phase.Down:
                UpdateDown();
                break;

            case Phase.GettingUp:
                UpdateGettingUp();
                break;
        }
    }

    private void UpdateSplat()
    {
        timer += Time.deltaTime;

        if (timer >= splatAnimation.duration)
        {
            EnterDown();
        }
    }

    private void EnterDown()
    {
        phase = Phase.Down;
        timer = 0f;

        context.Motor.StopHorizontalMovement();

        context.Animator.Play(
            downAnimation.animationState,
            0.08f);
    }

    private void UpdateDown()
    {
        timer += Time.deltaTime;

        if (timer >= downAnimation.duration)
        {
            EnterGettingUp();
        }
    }

    private void EnterGettingUp()
    {
        phase = Phase.GettingUp;
        timer = 0f;

        context.Animator.Play(
            getUpAnimation.animationState,
            0.08f);
    }

    private void UpdateGettingUp()
    {
        timer += Time.deltaTime;

        if (timer >= getUpAnimation.duration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.UnlockMovementInput();

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