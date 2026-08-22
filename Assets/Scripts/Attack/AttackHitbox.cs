using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackHitbox : MonoBehaviour
{
    private SphereCollider hitbox;

    private int damage;

    private readonly HashSet<IDamageable> hitTargets =
        new HashSet<IDamageable>();

    public string HitboxName => gameObject.name;

    private void Awake()
    {
        hitbox = GetComponent<SphereCollider>();

        hitbox.isTrigger = true;
        hitbox.enabled = false;
    }

    public void Activate(int damage)
    {
        this.damage = damage;

        hitTargets.Clear();

        hitbox.enabled = true;
    }

    public void Deactivate()
    {
        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null)
            return;

        if (hitTargets.Contains(damageable))
            return;

        hitTargets.Add(damageable);

        damageable.TakeDamage(damage);
    }

    void OnDrawGizmos()
    {
        if (!hitbox) return;
        Gizmos.color = Color.red;
        if (hitbox.enabled)
        {
            Gizmos.DrawSphere(transform.position, hitbox.radius);
        }
    }
}

public enum HitboxType
{
    LeftHand,
    RightHand,
    LeftFoot,
    RightFoot
}