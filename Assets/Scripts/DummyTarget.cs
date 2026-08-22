using UnityEngine;

public class DummyTarget : MonoBehaviour, IDamageable
{
    public GameObject hitDebug;
    public float debugTime = 0.1f;

    float timer = 1f;

    public void TakeDamage(int damage)
    {
        Debug.Log(
            $"{gameObject.name} recibió {damage} de daño. ");
        timer = 0f;
        hitDebug.SetActive(true);
    }

    void Update()
    {
        if (timer > debugTime)
            hitDebug.SetActive(false);
        else
            timer += Time.deltaTime;
    }
}