using UnityEngine;

public class TeleporterTrigger : MonoBehaviour
{
    [SerializeField] private float zPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var c = other.GetComponent<CharacterController>();
        c.enabled = false;
        var motor = c.GetComponent<CharacterMotor>();
        motor.LockMovement();

        other.gameObject.transform.position = new Vector3(
            other.gameObject.transform.position.x,
            other.gameObject.transform.position.y,
            other.gameObject.transform.position.z + zPosition);

        c.enabled = true;
        motor.UnlockMovement();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 v = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z + zPosition);
        Gizmos.DrawSphere(v, .1f);
    }
}