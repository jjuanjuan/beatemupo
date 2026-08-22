using UnityEngine;

public class KnockdownState : CharacterState
{
    private enum Phase
    {
        Hit,
        Falling,
        Down,
        GettingUp
    }

    private Phase phase;
    private float timer;

    private HitReactionDefinition hitAnimation;
    private HitReactionDefinition downAnimation;
    private HitReactionDefinition getUpAnimation;

    public KnockdownState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;
        phase = Phase.Hit;

        hitAnimation =
            context.Damage.CurrentHitDefinition;

        downAnimation =
            context.Damage.KnockedDownDefinition;

        getUpAnimation =
            context.Damage.GetUpDefinition;

        context.Motor.LockMovementInput();

        if (hitAnimation != null)
        {
            context.Animator.Play(
                hitAnimation.animationState,
                0f);
        }
        else
        {
            EnterFalling();
        }
    }

    public override void Update()
    {
        switch (phase)
        {
            case Phase.Hit:
                UpdateHit();
                break;

            case Phase.Falling:
                UpdateFalling();
                break;

            case Phase.Down:
                UpdateDown();
                break;

            case Phase.GettingUp:
                UpdateGettingUp();
                break;
        }
    }

    private void UpdateHit()
    {
        timer += Time.deltaTime;

        if (hitAnimation == null ||
            hitAnimation.animationClip == null)
        {
            EnterFalling();
            return;
        }

        if (timer >= hitAnimation.Duration)
        {
            EnterFalling();
        }
    }

    private void EnterFalling()
    {
        phase = Phase.Falling;
        timer = 0f;

        context.Animator.Play(
            "KnockdownFlying",
            0.08f);
    }

    private void UpdateFalling()
    {
        if (!context.Motor.Grounded)
            return;

        EnterDown();
    }

    private void EnterDown()
    {
        phase = Phase.Down;
        timer = 0f;

        context.Motor.StopHorizontalMovement();

        if (downAnimation != null)
        {
            context.Animator.Play(
                downAnimation.animationState,
                0.08f);
        }
    }

    private void UpdateDown()
    {
        timer += Time.deltaTime;

        if (downAnimation == null ||
            downAnimation.animationClip == null)
        {
            EnterGettingUp();
            return;
        }

        if (timer >= downAnimation.Duration)
        {
            EnterGettingUp();
        }
    }

    private void EnterGettingUp()
    {
        phase = Phase.GettingUp;
        timer = 0f;

        if (getUpAnimation != null)
        {
            context.Animator.Play(
                getUpAnimation.animationState,
                0.08f);
        }
        else
        {
            Finish();
        }
    }

    private void UpdateGettingUp()
    {
        timer += Time.deltaTime;

        if (getUpAnimation == null ||
            getUpAnimation.animationClip == null)
        {
            Finish();
            return;
        }

        if (timer >= getUpAnimation.Duration)
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