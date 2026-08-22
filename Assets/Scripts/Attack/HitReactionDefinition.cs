using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Hit Reaction Definition",
    fileName = "New Hit Reaction")]
public class HitReactionDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;
    public AnimationClip animationClip;

    public float Duration
    {
        get
        {
            if (animationClip == null)
                return 0f;

            return animationClip.length;
        }
    }
}