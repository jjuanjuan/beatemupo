using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Attack Definition",
    fileName = "New Attack")]
public class AttackDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;

    [Header("Timing")]
    public float duration = 0.8f;
    public float hitStart = 0.2f;
    public float hitEnd = 0.4f;
    public float comboStart = 0.25f;
    public float comboEnd = 0.55f;

    [Header("Combat")]
    public int damage = 10;
    public HitboxType hitbox;
}