using UnityEngine;

public class AerialKickState : CharacterState
{
    public AttackPhase Phase { get; private set; }
    private float timer;
    bool trailActive;

    public AerialKickState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        context.Motor.ConsumeAerialAttack();

        StartAttack(context.Combat.AerialKick);
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        UpdatePhase(context.Combat.AerialKick);
        UpdateSelfMovement(context.Combat.AerialKick);
        UpdateTrail(context.Combat.AerialKick);

        if (timer >= context.Combat.AerialKick.Duration)
        {
            FinishAttack();
        }
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

        context.Motor.AerialKick();
    }

    private void FinishAttack()
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();
        trailActive = false;
        context.Combat.EndAttack();
        context.Motor.EndAttack();

        stateMachine.ChangeState(
            context.States.Fall);
    }

    public override void Exit()
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();
        context.Motor.StopAttackMovement();
        context.Motor.EndAttack();
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

    private void UpdateSelfMovement(
    AttackDefinition attack)
    {
        bool shouldMove =
            timer >= attack.SelfMoveStart &&
            timer < attack.SelfMoveEnd;

        if (shouldMove)
        {
            context.Motor.StartAttackMovement(
                attack.selfMoveForce);
        }
        else
        {
            context.Motor.StopAttackMovement();
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
}