using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 EndPosition;
    [SerializeField] float speed = 2f;
    [SerializeField] float stopTime = 1f;

    private Vector3 originPosition;
    private bool stopped;
    private float timer;
    private bool goingToEnd;

    private Vector3 previousPosition;

    public Vector3 FrameDelta { get; private set; }

    void Awake()
    {
        originPosition = transform.position;

        previousPosition = transform.position;

        timer = 0f;
        goingToEnd = true;
        stopped = true;

        FrameDelta = Vector3.zero;
    }

    void Update()
    {
        FrameDelta = Vector3.zero;

        Vector3 previousPosition = transform.position;

        if (stopped)
        {
            timer += Time.deltaTime;

            if (timer >= stopTime)
            {
                timer = 0f;
                stopped = false;
            }
        }
        else
        {
            if (goingToEnd)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    EndPosition + originPosition,
                    speed * Time.deltaTime
                );

                if (transform.position == EndPosition + originPosition)
                {
                    goingToEnd = false;
                    stopped = true;
                    timer = 0f;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    originPosition,
                    speed * Time.deltaTime
                );

                if (transform.position == originPosition)
                {
                    goingToEnd = true;
                    stopped = true;
                    timer = 0f;
                }
            }
        }

        FrameDelta = transform.position - previousPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, .5f);

        if (Application.isPlaying)
        {
            Gizmos.DrawLine(
                originPosition,
                EndPosition + originPosition);
        }
        else
        {
            Gizmos.DrawLine(
                transform.position,
                EndPosition + transform.position);
        }

        Gizmos.color = new Color(.2f, .2f, .5f, .5f);

        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(
                EndPosition + originPosition,
                transform.localScale);

            Gizmos.DrawWireCube(
                originPosition,
                transform.localScale);
        }
        else
        {
            Gizmos.DrawWireCube(
                EndPosition + transform.position,
                transform.localScale);
        }
    }
}