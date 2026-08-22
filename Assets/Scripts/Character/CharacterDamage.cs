using UnityEngine;
using System.Collections.Generic;

public class CharacterDamage : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Hit Reactions")]
    [SerializeField] private List<HitReactionDefinition> headHits = new();
    [SerializeField] private List<HitReactionDefinition> bodyHits = new();
    [SerializeField] private HitReactionDefinition hitKnockdown;
    [SerializeField] private HitReactionDefinition knockedDown;
    [SerializeField] private HitReactionDefinition getUp;

    [Header("Death")]
    [SerializeField] private float airDeathTimeout = 5f;
    [SerializeField] private float disableDelay = 2f;

    public float AirDeathTimeout => airDeathTimeout;
    public float DisableDelay => disableDelay;

    private int currentHealth;
    private CharacterContext context;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private HitReactionDefinition lastHeadHit;
    private HitReactionDefinition lastBodyHit;

    public HitReaction CurrentHitReaction { get; private set; }
    private HitReactionDefinition currentHitDefinition;

    public HitReactionDefinition CurrentHitDefinition =>
        currentHitDefinition;
    public HitReactionDefinition KnockedDownDefinition =>
        knockedDown;
    public HitReactionDefinition GetUpDefinition =>
        getUp;

    public bool IsDead => currentHealth <= 0;

    public void Initialize(CharacterContext context)
    {
        this.context = context;
        currentHealth = maxHealth;
    }

    public void TakeDamage(HitData hit)
    {
        if (IsDead)
            return;

        currentHealth -= hit.damage;

        context.Motor.FacePosition(
            hit.attackerPosition);

        ApplyKnockback(
            hit.attackerPosition,
            hit.knockback,
            hit.knockbackUp);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        ReactToHit(hit.reaction);

        Debug.Log($"{name} got hit for {hit.damage}.");
    }

    private void ReactToHit(HitReaction reaction)
    {
        CurrentHitReaction = reaction;

        switch (reaction)
        {
            case HitReaction.Head:
                currentHitDefinition =
                    GetRandomVariation(
                        headHits,
                        ref lastHeadHit);

                context.Character.StateMachine.ChangeState(
                    context.States.Hit);
                break;

            case HitReaction.Chest:
                currentHitDefinition =
                    GetRandomVariation(
                        bodyHits,
                        ref lastBodyHit);

                context.Character.StateMachine.ChangeState(
                    context.States.Hit);
                break;

            case HitReaction.Knockdown:
                currentHitDefinition = hitKnockdown;

                context.Character.StateMachine.ChangeState(
                    context.States.Knockdown);
                break;
        }
    }

    private HitReactionDefinition GetRandomVariation(
        List<HitReactionDefinition> variations,
        ref HitReactionDefinition lastHit)
    {
        if (variations == null || variations.Count == 0)
            return null;

        if (variations.Count == 1)
        {
            lastHit = variations[0];
            return lastHit;
        }

        int index;

        do
        {
            index = Random.Range(0, variations.Count);
        }
        while (variations[index] == lastHit);

        lastHit = variations[index];

        return lastHit;
    }

    private void ApplyKnockback(
        Vector3 attackerPosition,
        float horizontalForce,
        float verticalForce)
    {
        Vector3 direction =
            transform.position - attackerPosition;

        context.Motor.ApplyKnockback(
            direction,
            horizontalForce,
            verticalForce);
    }

    private void Die()
    {
        context.Character.StateMachine.ChangeState(
            context.States.Death);
    }
}