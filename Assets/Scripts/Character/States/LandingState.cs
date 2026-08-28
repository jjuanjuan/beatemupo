using UnityEngine;

public class LandingState : CharacterState
{
    private float timer;
    private float duration;

    private float intensity;

    public LandingState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        float fallTime =
            context.Motor.LastFallTime;

        intensity =
            Mathf.InverseLerp(
                context.Motor.MinFallTime,
                context.Motor.MaxFallTime,
                fallTime);

        duration =
            Mathf.Lerp(
                context.Motor.MinLandingDuration,
                context.Motor.MaxLandingDuration,
                intensity);

        context.Motor.StartLandingMovement();

        context.Animator.SetLandingIntensity(
            intensity);

        context.Animator.Play(
            "Landing",
            0.05f);

        context.Projection.Deactivate();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Animator.SetLandingIntensity(0f);

        context.Motor.EndLandingMovement();

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
        context.Motor.EndLandingMovement();

        context.Animator.SetLandingIntensity(0f);
    }
}