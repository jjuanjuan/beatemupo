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

    [Header("Aerial Attacks")]
    [SerializeField] private AttackDefinition aerialKick;
    [SerializeField] private AttackDefinition groundPound;
    [SerializeField] private AnimationDefinition groundPoundStart;
    [SerializeField] private AnimationDefinition groundPoundGetUp;
    [SerializeField] ParticleSystem groundPoundParticles;

    [Header("Hitboxes")]
    [SerializeField] private AttackHitbox leftHandHitbox;
    [SerializeField] private AttackHitbox rightHandHitbox;
    [SerializeField] private AttackHitbox leftFootHitbox;
    [SerializeField] private AttackHitbox rightFootHitbox;
    [SerializeField] private AttackHitbox headHitbox;
    [SerializeField] private AttackHitbox bodyHitbox;

    [Header("Attack Trails")]
    [SerializeField] private AttackTrail leftHandTrail;
    [SerializeField] private AttackTrail rightHandTrail;
    [SerializeField] private AttackTrail leftFootTrail;
    [SerializeField] private AttackTrail rightFootTrail;
    [SerializeField] private AttackTrail headTrail;
    [SerializeField] private AttackTrail bodyTrail;

    public AttackDefinition CurrentAttack { get; private set; }

    public int ComboIndex { get; private set; }

    public AttackDefinition Punch =>
        GetAttack(punches);

    public AttackDefinition Kick =>
        GetAttack(kicks);

    private Character character;

    public AttackDefinition AerialKick => aerialKick;
    public AttackDefinition GroundPound => groundPound;
    public AnimationDefinition GroundPoundStart =>
        groundPoundStart;
    public AnimationDefinition GroundPoundGetUp =>
        groundPoundGetUp;
    public ParticleSystem GroundPoundParticles =>
        groundPoundParticles;

    private void Awake()
    {
        character = GetComponent<Character>();

        leftHandHitbox.Initialize(character);
        rightHandHitbox.Initialize(character);
        leftFootHitbox.Initialize(character);
        rightFootHitbox.Initialize(character);
        headHitbox.Initialize(character);
        bodyHitbox.Initialize(character);
        groundPoundParticles.Stop();
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
            CurrentAttack,
            transform.position);
    }

    public void BeginImpactHitbox()
    {
        if (CurrentAttack == null)
            return;

        AttackHitbox hitbox =
            GetHitbox(CurrentAttack.impactHitbox);

        if (hitbox == null)
            return;

        hitbox.Activate(
            CurrentAttack,
            transform.position);
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

        hitbox =
            GetHitbox(CurrentAttack.impactHitbox);

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

            case HitboxType.Head:
                return headHitbox;

            case HitboxType.Body:
                return bodyHitbox;

            default:
                return null;
        }
    }

    private AttackTrail GetTrail(HitboxType type)
    {
        switch (type)
        {
            case HitboxType.LeftHand:
                return leftHandTrail;

            case HitboxType.RightHand:
                return rightHandTrail;

            case HitboxType.LeftFoot:
                return leftFootTrail;

            case HitboxType.RightFoot:
                return rightFootTrail;

            case HitboxType.Head:
                return headTrail;

            case HitboxType.Body:
                return bodyTrail;

            default:
                return null;
        }
    }

    public void BeginTrail()
    {
        if (CurrentAttack == null)
            return;

        AttackTrail trail =
            GetTrail(CurrentAttack.hitbox);

        if (trail == null)
            return;

        trail.Play();
    }

    public void EndTrail()
    {
        if (CurrentAttack == null)
            return;

        AttackTrail trail =
            GetTrail(CurrentAttack.hitbox);

        if (trail == null)
            return;

        trail.Stop();
    }
}