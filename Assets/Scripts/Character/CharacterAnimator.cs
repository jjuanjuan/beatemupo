using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Animator animator;

    private int currentState;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    public void Play(
        string state,
        float transition = 0.1f)
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
        float transitionDuration
        )
    {
        int hash = Animator.StringToHash(state);

        currentState = hash;

        float normalizedTime = 0f;

        animator.CrossFade(
            hash,
            transitionDuration,
            0,
            normalizedTime);
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(
            SpeedHash,
            speed);
    }
}