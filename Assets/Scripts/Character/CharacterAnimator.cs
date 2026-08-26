using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;

    [Header("Definitions")]
    [SerializeField] public AnimationDefinition ledgeClimbAnimation;

    [SerializeField] public AnimationDefinition SplatAnimation;
    [SerializeField] public AnimationDefinition SplatDownAnimation;
    [SerializeField] public AnimationDefinition SplatGetUpAnimation;

    [SerializeField] public AnimationDefinition RollAnimation;

    [Header("For interactions")]
    [SerializeField] private float lookTargetHeight = 1.5f;
    [SerializeField] private float lookTransitionTime = 3f;

    private int currentState;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");
    private static readonly int FallTimeHash =
        Animator.StringToHash("FallTime");
    private static readonly int LandingIntensityHash =
        Animator.StringToHash("LandingIntensity");

    private Character lookTarget;
    private float currentLookWeight;

    public void Play(
        string state,
        float transition = 0.15f)
    {
        int hash = Animator.StringToHash(state);

        if (hash == currentState)
            return;

        currentState = hash;

        animator.CrossFade(
            hash,
            transition,
            0);
    }

    public void PlayAttack(
        string state,
        float transition,
        float normalizedTime)
    {
        int hash = Animator.StringToHash(state);

        currentState = hash;

        animator.CrossFade(
            hash,
            transition,
            0,
            normalizedTime);
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(SpeedHash, speed);
    }

    public void SetFallTime(float time)
    {
        animator.SetFloat(
            FallTimeHash,
            time);
    }

    public void SetLandingIntensity(float intensity)
    {
        animator.SetFloat(
            LandingIntensityHash,
            intensity);
    }
}