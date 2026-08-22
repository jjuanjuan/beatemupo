using UnityEngine;

public class AttackState : CharacterState
{
    public AttackPhase Phase { get; private set; }

    float timer;
    bool comboWindowOpen;

    public AttackState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        AttackDefinition attack =
            context.Combat.CurrentAttack;

        StartAttack(attack);
    }

    public override void Update()
    {
        AttackDefinition attack =
            context.Combat.CurrentAttack;

        timer += Time.deltaTime;

        UpdatePhase(attack);
        UpdateComboWindow(attack);
        UpdateSelfMovement(attack);

        if (comboWindowOpen)
        {
            TryCombo(attack);

            if (stateMachine.CurrentState != this)
                return;
        }

        if (timer >= attack.Duration)
        {
            FinishAttack();
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

    private void UpdateComboWindow(AttackDefinition attack)
    {
        comboWindowOpen =
            timer >= attack.ComboStart &&
            timer <= attack.ComboEnd;
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

    private void TryCombo(AttackDefinition attack)
    {
        if (context.Brain.PunchPressed)
        {
            context.Combat.AdvanceCombo();

            AttackDefinition nextAttack =
                context.Combat.Punch;

            if (nextAttack != null)
            {
                ExecuteNextAttack(nextAttack);
            }

            return;
        }

        if (context.Brain.KickPressed)
        {
            context.Combat.AdvanceCombo();

            AttackDefinition nextAttack =
                context.Combat.Kick;

            if (nextAttack != null)
            {
                ExecuteNextAttack(nextAttack);
            }

            return;
        }
    }

    private void StartAttack(AttackDefinition attack)
    {
        context.Combat.StartAttack(attack);

        timer = 0f;

        Phase = AttackPhase.Startup;

        context.Animator.PlayAttack(
            attack.animationState,
            0f,
            0f);

        context.Motor.LockMovementInput();
    }
    private void StartComboAttack(AttackDefinition attack)
    {
        context.Combat.StartAttack(attack);

        timer = 0f;

        Phase = AttackPhase.Startup;

        context.Animator.PlayAttack(
            attack.animationState,
            0.08f,
            0f);

        context.Motor.LockMovementInput();
    }
    
    private void ExecuteNextAttack(
        AttackDefinition nextAttack)
    {
        StartComboAttack(nextAttack);
    }

    private void FinishAttack()
    {
        context.Motor.StopAttackMovement();

        context.Combat.EndHitbox();
        context.Combat.EndAttack();
        context.Motor.EndAttack();

        Vector2 input =
            context.Brain.MoveInput;

        if (input.sqrMagnitude > 0.01f)
            stateMachine.ChangeState(context.States.Move);
        else
            stateMachine.ChangeState(context.States.Idle);
    }

    public override void Exit()
    {
        context.Motor.StopAttackMovement();
        context.Motor.EndAttack();
    }
}

public enum AttackPhase
{
    Startup,
    Active,
    Recovery
}