using UnityEngine;

public class VisibilityManager : MonoBehaviour
{
    public static VisibilityManager Instance { get; private set; }

    [SerializeField] private Transform exteriorRoot;
    private Renderer[] exteriorRenderers;
    private Building[] buildings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        exteriorRenderers =
            exteriorRoot.GetComponentsInChildren<Renderer>(true);

        buildings = FindObjectsOfType<Building>();
    }

    void Start()
    {
        SetExteriorVisible(true);
    }

    public void SetExteriorVisible(bool visible)
    {
        foreach (Renderer renderer in exteriorRenderers)
        {
            if (renderer != null)
                renderer.enabled = visible;
        }
        foreach (Building b in buildings)
        {
            b.SetExteriorVisible(visible);
        }
    }
}