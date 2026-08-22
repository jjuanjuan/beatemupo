using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Attack Definition",
    fileName = "New Attack")]
public class AttackDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;
    public AnimationClip animationClip;

    [Tooltip("Start frame configured in the FBX Animation Import Settings.")]
    [Min(0)]
    public int animationStartFrame = 0;

    [Header("Timing")]
    [Min(1)]
    public int hitStartFrame = 10;

    [Min(1)]
    public int hitEndFrame = 14;

    [Min(1)]
    public int comboStartFrame = 18;

    [Min(1)]
    public int comboEndFrame = 30;

    [Header("Combat")]
    public int damage = 10;
    public HitboxType hitbox;
    public HitReaction hitReaction;

    public float Duration
    {
        get
        {
            if (animationClip == null)
                return 0f;

            return animationClip.length;
        }
    }

    public float FrameRate
    {
        get
        {
            if (animationClip == null)
                return 0f;

            return animationClip.frameRate;
        }
    }

    public int FrameCount
    {
        get
        {
            if (animationClip == null)
                return 0;

            return Mathf.RoundToInt(
                animationClip.length * animationClip.frameRate);
        }
    }

    // Frames relativos al clip que realmente reproduce Unity.
    public int HitStartFrame => hitStartFrame - animationStartFrame;
    public int HitEndFrame => hitEndFrame - animationStartFrame;
    public int ComboStartFrame => comboStartFrame - animationStartFrame;
    public int ComboEndFrame => comboEndFrame - animationStartFrame;

    public float HitStart => FrameToTime(HitStartFrame);
    public float HitEnd => FrameToTime(HitEndFrame);
    public float ComboStart => FrameToTime(ComboStartFrame);
    public float ComboEnd => FrameToTime(ComboEndFrame);

    private float FrameToTime(int frame)
    {
        if (animationClip == null)
            return 0f;

        return frame / animationClip.frameRate;
    }
}

public enum HitReaction
{
    None,
    Head,
    Chest,
    Knockdown
}