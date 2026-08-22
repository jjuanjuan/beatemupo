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
        AttackPhase previousPhase = Phase;

        if (timer < attack.HitStart)
            Phase = AttackPhase.Startup;
        else if (timer < attack.HitEnd)
            Phase = AttackPhase.Active;
        else
            Phase = AttackPhase.Recovery;

        if (previousPhase == Phase)
            return;

        switch (Phase)
        {
            case AttackPhase.Startup:
                break;

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

    private void TryCombo(AttackDefinition attack)
    {
        if (context.Brain.PunchPressed)
        {
            ExecuteNextAttack(
                context.Combat.Punch);

            return;
        }

        if (context.Brain.KickPressed)
        {
            ExecuteNextAttack(
                context.Combat.Kick);

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

        context.Motor.StartAttack();
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

        context.Motor.StartAttack();
    }

    private void ExecuteNextAttack(
        AttackDefinition nextAttack)
    {
        StartComboAttack(nextAttack);
    }

    private void FinishAttack()
    {
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
        context.Motor.EndAttack();
    }
}

public enum AttackPhase
{
    Startup,
    Active,
    Recovery
}