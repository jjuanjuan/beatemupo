using UnityEngine;

public class CharacterTargeting : MonoBehaviour
{
    [Header("Vision Cone")]
    [SerializeField] private float visionRange = 4f;
    [SerializeField, Range(1f, 180f)]
    private float visionAngle = 90f;

    [Header("Detection")]
    [SerializeField] private LayerMask characterLayer;

    private readonly Collider[] results = new Collider[32];

    public Character FindClosestCharacter()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            visionRange,
            results,
            characterLayer);

        Character closest = null;
        float closestDistanceSqr = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider collider = results[i];

            if (collider == null)
                continue;

            Character character =
                collider.GetComponentInParent<Character>();

            if (character == null)
                continue;

            if (character == GetComponent<Character>())
                continue;

            Vector3 direction =
                character.transform.position -
                transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                continue;

            float distanceSqr =
                direction.sqrMagnitude;

            Vector3 normalizedDirection =
                direction.normalized;

            float angle =
                Vector3.Angle(
                    transform.forward,
                    normalizedDirection);

            if (angle > visionAngle * 0.5f)
                continue;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closest = character;
            }
        }

        return closest;
    }

    public void FaceTarget(Character target)
    {
        if (target == null)
            return;

        Vector3 direction =
            target.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation =
            Quaternion.LookRotation(direction);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position;
        origin.y += 0.1f;

        Gizmos.color = Color.yellow;

        Vector3 forward =
            transform.forward;

        Vector3 left =
            Quaternion.Euler(
                0f,
                -visionAngle * 0.5f,
                0f) * forward;

        Vector3 right =
            Quaternion.Euler(
                0f,
                visionAngle * 0.5f,
                0f) * forward;

        Gizmos.DrawLine(
            origin,
            origin + left * visionRange);

        Gizmos.DrawLine(
            origin,
            origin + right * visionRange);

        Gizmos.DrawLine(
            origin,
            origin + forward * visionRange);
    }
}