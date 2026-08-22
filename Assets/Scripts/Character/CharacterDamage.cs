using UnityEngine;

public class CharacterDamage : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private HitReactionDefinition hitHead;
    [SerializeField] private HitReactionDefinition hitChest;

    private int currentHealth;
    private CharacterContext context;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public HitReaction CurrentHitReaction { get; private set; }
    public HitReactionDefinition CurrentHitDefinition
    {
        get
        {
            switch (CurrentHitReaction)
            {
                case HitReaction.Head:
                    return hitHead;

                case HitReaction.Chest:
                    return hitChest;

                default:
                    return null;
            }
        }
    }

    public void Initialize(CharacterContext context)
    {
        this.context = context;
        currentHealth = maxHealth;
    }

    public void TakeDamage(HitData hit)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= hit.damage;

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
            case HitReaction.Chest:
                context.Character.StateMachine.ChangeState(
                    context.States.Hit);
                break;

            case HitReaction.Knockdown:
                context.Character.StateMachine.ChangeState(
                    context.States.Knockdown);
                break;
        }
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
        Debug.Log($"{name} died.");
    }
}