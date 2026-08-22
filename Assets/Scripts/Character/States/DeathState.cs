using UnityEngine;

public class DeathState : CharacterState
{
    private enum Phase
    {
        WaitingForGround,
        Dead
    }

    private Phase phase;
    private float timer;

    public DeathState(
        CharacterContext context,
        CharacterStateMachine stateMachine)
        : base(context, stateMachine)
    {
    }

    public override void Enter()
    {
        timer = 0f;

        context.Motor.LockMovement();

        if (context.Motor.Grounded)
        {
            Die();
        }
        else
        {
            phase = Phase.WaitingForGround;
        }
    }

    public override void Update()
    {
        switch (phase)
        {
            case Phase.WaitingForGround:
                UpdateWaitingForGround();
                break;

            case Phase.Dead:
                UpdateDead();
                break;
        }
    }

    private void UpdateWaitingForGround()
    {
        timer += Time.deltaTime;

        if (context.Motor.Grounded)
        {
            Die();
            return;
        }

        if (timer >= context.Damage.AirDeathTimeout)
        {
            Die();
        }
    }

    private void Die()
    {
        phase = Phase.Dead;
        timer = 0f;

        context.Motor.LockMovement();

        context.Motor.DisableCharacterController();

        context.Animator.Play(
            "Death",
            0f);
    }

    private void UpdateDead()
    {
        timer += Time.deltaTime;

        if (timer >= context.Damage.DisableDelay)
        {
            context.Character.gameObject.SetActive(false);
        }
    }

    public override void Exit()
    {
    }
}