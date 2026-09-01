using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float navMeshSearchRadius = 2f;

    public void Spawn()
    {
        GameObject enemy =
            Instantiate(
                prefab,
                transform.position,
                transform.rotation);

        NavMeshAgent agent =
            enemy.GetComponent<NavMeshAgent>();

        if (NavMesh.SamplePosition(
                transform.position,
                out NavMeshHit hit,
                navMeshSearchRadius,
                NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }
}