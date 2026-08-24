using UnityEngine;

public class BuildingTrigger : MonoBehaviour
{
    [SerializeField] private Building building;

    private void OnTriggerEnter(Collider other)
    {
        CharacterMotor motor =
            other.GetComponent<CharacterMotor>();

        if (motor == null)
            return;

        BuildingVisibilityManager.Instance
            .EnterBuilding(building);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterMotor motor =
            other.GetComponent<CharacterMotor>();

        if (motor == null)
            return;

        BuildingVisibilityManager.Instance
            .ExitBuilding(building);
    }
}