using System.Collections.Generic;
using UnityEngine;

public class BuildingVisibilityManager : MonoBehaviour
{
    public static BuildingVisibilityManager Instance { get; private set; }

    [SerializeField] private GameObject exteriorRoot;

    private readonly List<Building> buildings =
        new List<Building>();

    private Building currentBuilding;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        FindBuildings();

        SetOutsideState();
    }

    private void FindBuildings()
    {
        buildings.Clear();

        Building[] foundBuildings =
            FindObjectsByType<Building>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        foreach (Building building in foundBuildings)
        {
            if (!buildings.Contains(building))
                buildings.Add(building);
        }
    }

    public void EnterBuilding(Building building)
    {
        if (building == null)
            return;

        currentBuilding = building;

        if (exteriorRoot != null)
            exteriorRoot.SetActive(false);

        foreach (Building otherBuilding in buildings)
        {
            otherBuilding.SetInteriorVisible(
                otherBuilding == building);
            otherBuilding.SetExteriorVisible(false);
        }
    }

    public void ExitBuilding(Building building)
    {
        if (currentBuilding != building)
            return;

        currentBuilding = null;

        SetOutsideState();
    }

    private void SetOutsideState()
    {
        if (exteriorRoot != null)
            exteriorRoot.SetActive(true);

        foreach (Building building in buildings)
        {
            building.SetInteriorVisible(false);
            building.SetExteriorVisible(true);
        }
    }
}