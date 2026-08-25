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

        //SetInteriorVisible(false);
    }

    public void EnterBuilding()
    {
        if (playerInside)
            return;

        playerInside = true;

        VisibilityManager.Instance.SetInteriorBuilding(this);
        VisibilityManager.Instance.SetExteriorVisible(false);
    }

    public void ExitBuilding()
    {
        if (!playerInside)
            return;

        playerInside = false;

        VisibilityManager.Instance.ResetBuildingInteriors();
        VisibilityManager.Instance.SetExteriorVisible(true);
    }

    public void SetInteriorVisible(bool visible)
    {
        foreach (Renderer renderer in interiorRenderers)
        {
            if (renderer != null)
            {
                if (renderer.gameObject.tag != "IgnoreVisibilityChange")
                    renderer.enabled = visible;
            }
        }
    }
    public void SetExteriorVisible(bool visible)
    {
        foreach (Renderer renderer in exteriorRenderers)
        {
            if (renderer != null)
            {
                if (renderer.gameObject.tag != "IgnoreVisibilityChange")
                    renderer.enabled = visible;
            }
        }
    }
}