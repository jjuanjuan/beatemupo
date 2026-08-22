using UnityEngine;

public class HitState : CharacterState
{
    private float timer;

    public HitState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        HitReactionDefinition hit =
            context.Damage.CurrentHitDefinition;

        if (hit == null)
        {
            Finish();
            return;
        }

        context.Animator.Play(
            hit.animationState,
            0f);

        context.Motor.LockMovement();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        HitReactionDefinition hit =
            context.Damage.CurrentHitDefinition;

        if (hit == null)
        {
            Finish();
            return;
        }

        if (timer >= hit.Duration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.UnlockMovement();

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
        context.Motor.UnlockMovement();
    }
}