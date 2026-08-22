using UnityEngine;

public class KnockdownState : CharacterState
{
    private float timer;

    private HitReactionDefinition reaction;

    public KnockdownState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        reaction =
            context.Damage.CurrentHitDefinition;

        context.Motor.LockMovement();

        if (reaction != null)
        {
            context.Animator.Play(
                reaction.animationState,
                0f);
        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (reaction == null)
        {
            Finish();
            return;
        }

        if (timer >= reaction.Duration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.UnlockMovement();

        stateMachine.ChangeState(
            context.States.Idle);
    }

    public override void Exit()
    {
        context.Motor.UnlockMovement();
    }
}