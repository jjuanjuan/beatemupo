using UnityEngine;

public class HitState : CharacterState
{
    private float timer;

    // Por ahora, duración aproximada del hit.
    // Después podemos obtenerla directamente del AnimationClip.
    private const float HitDuration = 0.4f;

    public HitState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        context.Motor.StartAttack();

        switch (context.Damage.CurrentHitReaction)
        {
            case HitReaction.Head:
                context.Animator.Play("HitHead", 0f);
                break;

            case HitReaction.Chest:
                context.Animator.Play("HitChest", 0f);
                break;
        }
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        context.Motor.Move(Vector2.zero);

        if (timer >= HitDuration)
        {
            Finish();
        }
    }

    private void Finish()
    {
        context.Motor.EndAttack();

        Vector2 input =
            context.Brain.MoveInput;

        if (input.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(
                context.States.Move);
        else
            stateMachine.ChangeState(
                context.States.Idle);
    }

    public override void Exit()
    {
        context.Motor.EndAttack();
    }
}