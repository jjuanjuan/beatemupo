using UnityEngine;

[CreateAssetMenu(
    fileName = "AnimationDefinition",
    menuName = "Character/Animation Definition")]
public class AnimationDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;
    public float duration = .5f;
    [Tooltip("Alternative duration")] public float optionalDuration = 1f;

    [Header("Movement")]
    public AnimationCurve movementX =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public AnimationCurve movementY =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public AnimationCurve movementZ =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
}