using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 10;
    [SerializeField] GameObject Model;
    [SerializeField] GameObject HitModel;
    [Tooltip("Puede ser null")][SerializeField] GameObject DestroyedModel;
    [SerializeField] float ShakeIntensity = .1f;
    [SerializeField] float ShakeTime = .1f;
    [SerializeField] ParticleSystem DestroyParticles;

    Collider Collider;

    int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    float timer;

    void Awake()
    {
        HitModel.SetActive(false);
        if (DestroyedModel != null)
            DestroyedModel.SetActive(false);
        currentHealth = maxHealth;
        Collider = GetComponent<Collider>();
        timer = ShakeTime;
        DestroyParticles.Stop();
    }

    public bool IsDead => currentHealth <= 0;

    void Update()
    {
        if (IsDead) return;

        timer += Time.deltaTime;
        if (timer > ShakeTime)
        {
            HitModel.SetActive(false);
            Model.SetActive(true);
        }
    }

    public void TakeDamage(HitData hit)
    {
        if (IsDead)
            return;

        currentHealth -= hit.damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        timer = 0f;
        HitModel.SetActive(true);
        Model.SetActive(false);
        var x = Random.Range(-ShakeIntensity, ShakeIntensity);
        var y = Random.Range(-ShakeIntensity, ShakeIntensity);
        var z = Random.Range(-ShakeIntensity, ShakeIntensity);
        HitModel.transform.position = transform.position
            + new Vector3(x, y, z);

        Debug.Log($"{name} got hit for {hit.damage}.");
    }

    private void Die()
    {
        Model.SetActive(false);
        HitModel.SetActive(false);
        if (DestroyedModel != null)
            DestroyedModel.SetActive(true);
        Collider.enabled = false;
        if (DestroyParticles)
            DestroyParticles.Play();
    }
}
