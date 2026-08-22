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

    [Header("Knockback")]
    public float knockback = 3f;
    public float knockbackUp = 0f;
    public HitReaction hitReaction;

    [Header("Movement")]
    [Min(0)]
    public int selfMoveStartFrame = 0;
    [Min(0)]
    public int selfMoveEndFrame = 0;
    [Min(0f)]
    public float selfMoveForce = 0f;

    [Header("Trail")]
    [Min(0)]
    public int trailStartFrame = 0;
    [Min(0)]
    public int trailEndFrame = 0;

    public float Duration
    {
        get
        {
            if (animationClip == null)
                return 0f;

            return animationClip.length;
        }
    }

    public int SelfMoveStartFrame
    {
        get
        {
            return selfMoveStartFrame - animationStartFrame;
        }
    }

    public int SelfMoveEndFrame
    {
        get
        {
            return selfMoveEndFrame - animationStartFrame;
        }
    }

    public float SelfMoveStart
    {
        get
        {
            return FrameToTime(SelfMoveStartFrame);
        }
    }

    public float SelfMoveEnd
    {
        get
        {
            return FrameToTime(SelfMoveEndFrame);
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
    public int TrailStartFrame => trailStartFrame - animationStartFrame;
    public int TrailEndFrame => trailEndFrame - animationStartFrame;

    public float HitStart => FrameToTime(HitStartFrame);
    public float HitEnd => FrameToTime(HitEndFrame);
    public float ComboStart => FrameToTime(ComboStartFrame);
    public float ComboEnd => FrameToTime(ComboEndFrame);
    public float TrailStart => FrameToTime(TrailStartFrame);
    public float TrailEnd => FrameToTime(TrailEndFrame);

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