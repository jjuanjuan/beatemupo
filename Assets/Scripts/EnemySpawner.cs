using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
