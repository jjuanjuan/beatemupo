using UnityEngine;

public class KnockdownState : CharacterState
{
    private enum Phase
    {
        Falling,
        Down,
        GettingUp
    }

    private Phase phase;
    private float timer;

    private const float DownDuration = 1.5f;
    private const float GetUpDuration = 1.5f;

    public KnockdownState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;
        phase = Phase.Falling;

        context.Motor.LockMovementInput();

        context.Animator.Play(
            "KnockdownFlying",
            0f);
    }

    public override void Update()
    {
        switch (phase)
        {
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

        var knockdown =
            context.Damage.KnockedDownDefinition;

        if (knockdown != null)
        {
            context.Animator.Play(
                knockdown.animationState,
                0f);
        }
    }
    
    private void UpdateDown()
    {
        timer += Time.deltaTime;

        if (timer >= DownDuration)
        {
            EnterGettingUp();
        }
    }

    private void EnterGettingUp()
    {
        phase = Phase.GettingUp;
        timer = 0f;

        context.Animator.Play(
            "GetUp",
            0f);
    }

    private void UpdateGettingUp()
    {
        timer += Time.deltaTime;

        if (timer >= GetUpDuration)
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