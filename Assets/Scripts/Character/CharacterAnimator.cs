using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    int currentState;

    static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    public void Play(string state, float transition = .1f)
    {
        int hash = Animator.StringToHash(state);

        if (hash == currentState)
            return;

        currentState = hash;

        animator.CrossFade(hash, transition);
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(SpeedHash, speed);
    }
}