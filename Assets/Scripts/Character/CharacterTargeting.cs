using UnityEngine;

public class CharacterTargeting : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask characterLayer;

    private readonly Collider[] results = new Collider[32];

    public Character FindClosestCharacter()
    {
        return FindClosestCharacter(detectionRange);
    }

    public Character FindClosestCharacter(float range)
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            range,
            results,
            characterLayer);

        Character closest = null;
        float closestDistanceSqr = float.MaxValue;

        Character self =
            GetComponent<Character>();

        for (int i = 0; i < count; i++)
        {
            Collider collider = results[i];

            if (collider == null)
                continue;

            Character character =
                collider.GetComponentInParent<Character>();

            if (character == null)
                continue;

            if (character == self)
                continue;

            Vector3 direction =
                character.transform.position -
                transform.position;

            direction.y = 0f;

            float distanceSqr =
                direction.sqrMagnitude;

            if (distanceSqr < 0.001f)
                continue;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closest = character;
            }
        }

        return closest;
    }
}