using UnityEngine;

public struct HitData
{
    public int damage;
    public HitReaction reaction;

    public Vector3 attackerPosition;

    public float knockback;
    public float knockbackUp;
}