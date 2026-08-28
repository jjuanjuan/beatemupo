using UnityEngine;

public class GroundPoundState : CharacterState
{
    private enum Phase
    {
        Start,
        Falling,
        GettingUp
    }

    private Phase phase;
    private float timer;

    public AttackPhase AttackPhase { get; private set; }

    private bool trailActive;

    public GroundPoundState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;
        phase = Phase.Start;

        context.Motor.ConsumeAerialAttack();

        context.Motor.LockMovementInput();

        context.Motor.StopHorizontalMovement();
        context.Motor.StopVerticalMovement();

        context.Animator.Play(
            context.Combat.GroundPoundStart.animationState,
            0.05f);
    }

    public override void Update()
    {
        switch (phase)
        {
            case Phase.Start:
                UpdateStart();
                break;

            case Phase.Falling:
                UpdateFalling();
                break;

            case Phase.GettingUp:
                UpdateGettingUp();
                break;
        }
    }

    private void UpdateStart()
    {
        timer += Time.deltaTime;

        context.Motor.StopHorizontalMovement();
        context.Motor.StopVerticalMovement();

        if (timer >= context.Combat.GroundPoundStart.duration)
            EnterFalling();
    }

    private void EnterFalling()
    {
        phase = Phase.Falling;
        timer = 0f;

        StartGroundPound();
    }

    private void UpdateFalling()
    {
        timer += Time.deltaTime;

        UpdatePhase(context.Combat.GroundPound);
        UpdateTrail(context.Combat.GroundPound);

        context.Motor.UpdateFallTime();

        if (context.Motor.Grounded)
        {
            context.Motor.EndFall();
            EnterGettingUp();
        }
    }

    private void StartGroundPound()
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();

        trailActive = false;

        context.Combat.StartAttack(context.Combat.GroundPound);

        context.Animator.PlayAttack(
            context.Combat.GroundPound.animationState,
            0.05f,
            0f);

        context.Motor.GroundPound();
        context.Motor.StartFall();
    }

    private void UpdatePhase(
        AttackDefinition attack)
    {
        AttackPhase newPhase;

        if (timer < attack.HitStart)
        {
            newPhase = AttackPhase.Startup;
        }
        else if (timer < attack.HitEnd)
        {
            newPhase = AttackPhase.Active;
        }
        else
        {
            newPhase = AttackPhase.Recovery;
        }

        if (newPhase == AttackPhase)
            return;

        AttackPhase = newPhase;

        switch (AttackPhase)
        {
            case AttackPhase.Active:
                context.Combat.BeginHitbox();
                break;

            case AttackPhase.Recovery:
                context.Combat.EndHitbox();
                break;
        }
    }

    private void UpdateTrail(
        AttackDefinition attack)
    {
        bool shouldBeActive =
            timer >= attack.TrailStart &&
            timer < attack.TrailEnd;

        if (shouldBeActive == trailActive)
            return;

        trailActive = shouldBeActive;

        if (trailActive)
        {
            context.Combat.BeginTrail();
        }
        else
        {
            context.Combat.EndTrail();
        }
    }

    private void EnterGettingUp()
    {
        phase = Phase.GettingUp;
        timer = 0f;

        context.Combat.EndHitbox();
        context.Combat.EndTrail();

        trailActive = false;

        context.Motor.StopHorizontalMovement();
        context.Motor.StopVerticalMovement();

        context.Animator.Play(
            context.Combat.GroundPoundGetUp.animationState,
            0.08f);
        context.Combat.GroundPoundParticles.Play();

        context.Combat.BeginImpactHitbox();

        context.Projection.Deactivate();
    }

    private void UpdateGettingUp()
    {
        timer += Time.deltaTime;

        if (timer >= context.Combat.GroundPound.impactAdditionalDuration)
        {
            context.Combat.EndHitbox();
        }

        if (context.Motor.LastFallTime
            < context.Motor.GroundPoundHardFallThreshold)
        {
            if (timer >= context.Combat.GroundPoundGetUp.duration)
            {
                Finish();
            }
        }
        else
        {
            if (timer >= context.Combat.GroundPoundGetUp.optionalDuration)
            {
                Finish();
            }
        }
    }

    private void Finish()
    {
        context.Combat.EndHitbox();
        context.Combat.EndTrail();

        trailActive = false;

        context.Combat.EndAttack();
        context.Motor.EndAttack();

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
        context.Combat.EndHitbox();
        context.Combat.EndTrail();

        trailActive = false;

        context.Motor.StopAttackMovement();
        context.Motor.EndAttack();
        context.Motor.UnlockMovementInput();
    }
}