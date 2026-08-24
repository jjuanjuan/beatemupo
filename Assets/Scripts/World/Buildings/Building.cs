using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] Transform interiorRoot;
    [SerializeField] Transform exteriorRoot;

    private bool playerInside;
    private Renderer[] interiorRenderers;
    private Renderer[] exteriorRenderers;

    private void Awake()
    {
        interiorRenderers =
            interiorRoot.GetComponentsInChildren<Renderer>(true);
        exteriorRenderers =
            exteriorRoot.GetComponentsInChildren<Renderer>(true);

        SetInteriorVisible(false);
    }

    public void EnterBuilding()
    {
        if (playerInside)
            return;

        playerInside = true;

        SetInteriorVisible(true);

        VisibilityManager.Instance.SetExteriorVisible(false);
    }

    public void ExitBuilding()
    {
        if (!playerInside)
            return;

        playerInside = false;

        SetInteriorVisible(false);

        VisibilityManager.Instance.SetExteriorVisible(true);
    }

    private void SetInteriorVisible(bool visible)
    {
        foreach (Renderer renderer in interiorRenderers)
        {
            if (renderer != null)
                renderer.enabled = visible;
        }
    }
    public void SetExteriorVisible(bool visible)
    {
        foreach (Renderer renderer in exteriorRenderers)
        {
            if (renderer != null)
                renderer.enabled = visible;
        }
    }
}