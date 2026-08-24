using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] GameObject interiorRoot;
    [SerializeField] GameObject exteriorRoot;

    private void Awake()
    {
        SetInteriorVisible(false);
    }

    public void SetInteriorVisible(bool visible)
    {
        if (interiorRoot == null)
            return;

        interiorRoot.SetActive(visible);
    }
    public void SetExteriorVisible(bool visible)
    {
        if (exteriorRoot == null)
            return;

        exteriorRoot.SetActive(visible);
    }
}