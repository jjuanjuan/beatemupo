using UnityEngine;

public class GroundPoundState : CharacterState
{
    public AttackPhase Phase { get; private set; }

    float timer;
    bool trailActive;

    public GroundPoundState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        context.Motor.ConsumeAerialAttack();

        StartAttack(context.Combat.GroundPound);
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        UpdatePhase(context.Combat.CurrentAttack);
        UpdateTrail(context.Combat.CurrentAttack);

        if (context.Motor.Grounded)
        {
            FinishAttack();
            return;
        }
    }

    private void UpdatePhase(AttackDefinition attack)
    {
        AttackPhase newPhase;

        if (timer < attack.HitStart)
            newPhase = AttackPhase.Startup;
        else if (timer < attack.HitEnd)
            newPhase = AttackPhase.Active;
        else
            newPhase = AttackPhase.Recovery;

        if (newPhase == Phase)
            return;

        Phase = newPhase;

        switch (Phase)
        {
            case AttackPhase.Active:
                context.Combat.BeginHitbox();
                break;

            case AttackPhase.Recovery:
                context.Combat.EndHitbox();
                break;
        }
    }

    private void UpdateTrail(AttackDefinition attack)
    {
        bool shouldBeActive =
            timer >= attack.TrailStart &&
            timer < attack.TrailEnd;

        if (shouldBeActive == trailActive)
            return;

        trailActive = shouldBeActive;

        if (trailActive)
            context.Combat.BeginTrail();
        else
            context.Combat.EndTrail();
    }

    private void StartAttack(
        AttackDefinition attack)
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();
        trailActive = false;

        context.Combat.StartAttack(attack);

        context.Animator.PlayAttack(
            attack.animationState,
            0.05f,
            0f);

        context.Motor.LockMovementInput();

        context.Motor.GroundPound();
    }

    private void FinishAttack()
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();
        trailActive = false;
        context.Combat.EndAttack();
        context.Motor.EndAttack();

        if (context.Motor.Grounded)
        {
            stateMachine.ChangeState(
                context.States.Idle);

            return;
        }

        stateMachine.ChangeState(
            context.States.Fall);
    }

    public override void Exit()
    {
        context.Combat.EndTrail();
        trailActive = false;
        context.Motor.StopAttackMovement();
        context.Motor.EndAttack();
    }
}