using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitbox : MonoBehaviour
{
    private SphereCollider hitbox;
    private int damage;
    private Character owner;
    private HitReaction reaction;
    private Vector3 attackerPosition;
    private float knockback;
    private float knockbackUp;
    private HitData hitData;

    private readonly HashSet<IDamageable> hitTargets =
        new HashSet<IDamageable>();

    public string HitboxName => gameObject.name;

    private void Awake()
    {
        hitbox = GetComponent<SphereCollider>();

        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    public void Initialize(Character owner)
    {
        this.owner = owner;
    }

    public void Activate(
        AttackDefinition attack,
        Vector3 attackerPosition)
    {
        hitData = new HitData
        {
            damage = attack.damage,
            reaction = attack.hitReaction,
            attackerPosition = attackerPosition,
            knockback = attack.knockback,
            knockbackUp = attack.knockbackUp
        };

        hitTargets.Clear();
        hitbox.enabled = true;
    }
    public void Deactivate()
    {
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == owner.transform.root)
            return;

        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null)
            return;

        if (hitTargets.Contains(damageable))
            return;

        hitTargets.Add(damageable);

        damageable.TakeDamage(hitData);
    }

    private void OnDrawGizmos()
    {
        if (!hitbox)
            return;

        Vector3 center =
            transform.TransformPoint(hitbox.center);

        float radius =
            hitbox.radius *
            Mathf.Max(
                transform.lossyScale.x,
                transform.lossyScale.y,
                transform.lossyScale.z);

        Gizmos.color = hitbox.enabled
            ? new Color(1f, 0f, 0f, .4f)
            : new Color(.5f, .5f, .5f, .2f);

        Gizmos.DrawSphere(center, radius);
    }
}

public enum HitboxType
{
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot
}