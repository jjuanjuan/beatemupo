using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(DecalProjector))]
public class ShadowProjector : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float rayHeight = 5f;
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private float surfaceOffset = 0.02f;

    [Header("Shadow")]
    [SerializeField] float baseSize = 1.5f;
    [SerializeField] float minScale = 0.5f;
    [SerializeField] float maxHeight = 6f;

    Transform parent;
    DecalProjector projector;
    [SerializeField] CharacterController controller;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
        parent = transform.parent;
    }

    void LateUpdate()
    {
        Vector3 origin = controller.bounds.min + Vector3.up * rayHeight;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance, groundMask))
        {
            // Distancia al suelo
            float height = parent.position.y - hit.point.y;

            // Escala
            float t = Mathf.Clamp01(height / maxHeight);
            float scale = Mathf.Lerp(baseSize, baseSize * minScale, t);

            projector.size = new Vector3(scale, scale, projector.size.z);
        }
    }
}