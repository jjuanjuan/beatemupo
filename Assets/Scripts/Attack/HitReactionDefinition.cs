using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Hit Reaction Definition",
    fileName = "New Hit Reaction")]
public class HitReactionDefinition : ScriptableObject
{
    public string animationState;
    public AnimationClip animationClip;
}