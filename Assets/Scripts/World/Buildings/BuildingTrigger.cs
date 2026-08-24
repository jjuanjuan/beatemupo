using UnityEngine;

public class BuildingTrigger : MonoBehaviour
{
    [SerializeField] private Building building;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        building.EnterBuilding();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        building.ExitBuilding();
    }
}