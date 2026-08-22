using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [Header("Attacks")]
    [SerializeField] private AttackDefinition punch;
    [SerializeField] private AttackDefinition kick;

    [Header("Hitboxes")]
    [SerializeField] private AttackHitbox leftHandHitbox;
    [SerializeField] private AttackHitbox rightHandHitbox;
    [SerializeField] private AttackHitbox leftFootHitbox;
    [SerializeField] private AttackHitbox rightFootHitbox;

    public AttackDefinition CurrentAttack { get; private set; }

    public AttackDefinition Punch => punch;
    public AttackDefinition Kick => kick;

    public void StartAttack(AttackDefinition attack)
    {
        CurrentAttack = attack;
    }

    public void EndAttack()
    {
        EndHitbox();

        CurrentAttack = null;
    }

    public void BeginHitbox()
    {
        if (CurrentAttack == null)
            return;

        AttackHitbox hitbox =
            GetHitbox(CurrentAttack.hitbox);

        if (hitbox == null)
            return;

        hitbox.Activate(
            CurrentAttack.damage);
    }
    public void EndHitbox()
    {
        if (CurrentAttack == null)
            return;

        AttackHitbox hitbox =
            GetHitbox(CurrentAttack.hitbox);

        if (hitbox == null)
            return;

        hitbox.Deactivate();
    }

    private AttackHitbox GetHitbox(HitboxType type)
    {
        switch (type)
        {
            case HitboxType.LeftHand:
                return leftHandHitbox;

            case HitboxType.RightHand:
                return rightHandHitbox;

            case HitboxType.LeftFoot:
                return leftFootHitbox;

            case HitboxType.RightFoot:
                return rightFootHitbox;

            default:
                return null;
        }
    }
}