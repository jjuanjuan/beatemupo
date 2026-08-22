using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [Header("Punch Combo")]
    [SerializeField]
    private List<AttackDefinition> punches;

    [Header("Kick Combo")]
    [SerializeField]
    private List<AttackDefinition> kicks;

    [Header("Hitboxes")]
    [SerializeField] private AttackHitbox leftHandHitbox;
    [SerializeField] private AttackHitbox rightHandHitbox;
    [SerializeField] private AttackHitbox leftFootHitbox;
    [SerializeField] private AttackHitbox rightFootHitbox;

    public AttackDefinition CurrentAttack { get; private set; }

    public int ComboIndex { get; private set; }

    public AttackDefinition Punch =>
        GetAttack(punches);

    public AttackDefinition Kick =>
        GetAttack(kicks);

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();

        leftHandHitbox.Initialize(character);
        rightHandHitbox.Initialize(character);
        leftFootHitbox.Initialize(character);
        rightFootHitbox.Initialize(character);
    }

    public void StartAttack(AttackDefinition attack)
    {
        CurrentAttack = attack;
    }

    public void AdvanceCombo()
    {
        ComboIndex++;
    }

    public void ResetCombo()
    {
        ComboIndex = 0;
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

    private AttackDefinition GetAttack(
        List<AttackDefinition> attacks)
    {
        if (attacks == null || attacks.Count == 0)
            return null;

        return attacks[
            ComboIndex % attacks.Count
        ];
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