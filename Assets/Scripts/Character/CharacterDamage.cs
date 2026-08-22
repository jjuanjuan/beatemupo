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

    public void Initialize(CharacterContext context)
    {
        this.context = context;
        currentHealth = maxHealth;
    }

    public void TakeDamage(
        int damage,
        HitReaction reaction)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        ReactToHit(reaction);

        Debug.Log($"{name} got hit for {damage}.");
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

    private void Die()
    {
        Debug.Log($"{name} died.");
    }
}