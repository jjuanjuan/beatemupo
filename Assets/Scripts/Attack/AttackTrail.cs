using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    public string TrailName => gameObject.name;

    private void Awake()
    {
        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        Stop();
    }

    public void Play()
    {
        if (particles == null)
            return;

        particles.Clear();
        particles.Play();
    }

    public void Stop()
    {
        if (particles == null)
            return;

        particles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmitting);
    }
}