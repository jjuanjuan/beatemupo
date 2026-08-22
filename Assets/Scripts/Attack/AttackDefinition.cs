using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Attack Definition",
    fileName = "New Attack")]
public class AttackDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;

    [Header("Timing")]
    public float duration = 0.5f;

    public float hitStart = 0.15f;
    public float hitEnd = 0.3f;

    [Header("Movement")]
    public float movementSpeed = 0f;

    [Header("Combat")]
    public int damage = 10;
}