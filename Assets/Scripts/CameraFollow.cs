using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset =
        new Vector3(0, 12, -8);

    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        Vector3 desired =
            target.position + offset;

        transform.position =
            Vector3.Lerp(
                transform.position,
                desired,
                smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}