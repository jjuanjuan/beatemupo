using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [Header("Attacks")]
    [SerializeField] private AttackDefinition punch;
    [SerializeField] private AttackDefinition kick;

    public AttackDefinition CurrentAttack { get; private set; }

    public AttackDefinition Punch => punch;
    public AttackDefinition Kick => kick;

    public void StartAttack(AttackDefinition attack)
    {
        CurrentAttack = attack;
    }

    public void EndAttack()
    {
        CurrentAttack = null;
    }
}