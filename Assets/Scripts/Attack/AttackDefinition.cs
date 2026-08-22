using UnityEngine;

[CreateAssetMenu(
    menuName = "Character/Attack Definition",
    fileName = "New Attack")]
public class AttackDefinition : ScriptableObject
{
    [Header("Animation")]
    public string animationState;
    public AnimationClip animationClip;

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

    public float Duration
    {
        get
        {
            if (animationClip == null)
                return 0f;

            return animationClip.length;
        }
    }

    public float HitStart
    {
        get
        {
            return FrameToTime(hitStartFrame);
        }
    }

    public float HitEnd
    {
        get
        {
            return FrameToTime(hitEndFrame);
        }
    }

    public float ComboStart
    {
        get
        {
            return FrameToTime(comboStartFrame);
        }
    }

    public float ComboEnd
    {
        get
        {
            return FrameToTime(comboEndFrame);
        }
    }

    private float FrameToTime(int frame)
    {
        if (animationClip == null)
            return 0f;

        return frame / animationClip.frameRate;
    }
}