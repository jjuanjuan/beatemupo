using UnityEngine;
using UnityEngine.AI;

public class NpcBrain : MonoBehaviour, ICharacterBrain
{
    public Vector2 MoveInput { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool PunchPressed { get; private set; }
    public bool KickPressed { get; private set; }
    public bool RollPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    private Vector3 jumpDirection;

    public Vector3 JumpDirection =>
        jumpDirection;

    private NavMeshAgent agent;
    private CharacterMotor motor;

    [Header("Target")]
    [SerializeField]
    private Character target;

    [Header("Movement")]
    [SerializeField]
    private float stopDistance = 2f;

    [Header("Off Mesh Link")]
    [SerializeField]
    private float linkArrivalDistance = 0.5f;

    private bool traversingOffMeshLink;
    private bool waitingForPathRefresh;

    private Vector3 offMeshEndPosition;

    private Vector3 jumpTarget;
    private bool hasJumpTarget;

    public Vector3 JumpTarget =>
        jumpTarget;

    public bool HasJumpTarget =>
        hasJumpTarget;

    public void ConsumeJumpTarget()
    {
        hasJumpTarget = false;
    }

    private void Awake()
    {
        agent =
            GetComponent<NavMeshAgent>();

        motor =
            GetComponent<CharacterMotor>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.autoTraverseOffMeshLink = false;

        if (target == null)
        {
            target =
                GameObject.FindWithTag("Player")
                    .GetComponent<Character>();
        }
    }

    private void Update()
    {
        JumpPressed = false;
        PunchPressed = false;
        KickPressed = false;
        RollPressed = false;
        InteractPressed = false;

        UpdateOffMeshLink();
        UpdateOffMeshLinkCompletion();

        if (waitingForPathRefresh)
            return;

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (traversingOffMeshLink)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        agent.nextPosition =
            transform.position;

        float distanceToTarget =
            Vector3.Distance(
                transform.position,
                target.transform.position);

        if (distanceToTarget <= stopDistance)
        {
            MoveDirection = Vector3.zero;

            agent.ResetPath();

            return;
        }

        agent.SetDestination(
            target.transform.position);

        Vector3 direction =
            agent.desiredVelocity;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            MoveDirection =
                direction.normalized;
        }
        else
        {
            MoveDirection =
                Vector3.zero;
        }
    }

    private void UpdateOffMeshLink()
    {
        if (!agent.isOnOffMeshLink)
            return;

        if (traversingOffMeshLink)
            return;

        OffMeshLinkData link =
            agent.currentOffMeshLinkData;

        traversingOffMeshLink = true;

        offMeshEndPosition =
            link.endPos;

        Vector3 direction =
            offMeshEndPosition -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            jumpDirection =
                direction;

            MoveDirection =
                direction;
        }

        jumpTarget =
            offMeshEndPosition;

        hasJumpTarget =
            true;

        JumpPressed =
            true;
    }

    private void UpdateOffMeshLinkCompletion()
    {
        if (!traversingOffMeshLink)
            return;

        if (!motor.Grounded)
            return;

        Vector3 currentPosition =
            transform.position;

        Vector3 endPosition =
            offMeshEndPosition;

        currentPosition.y = 0f;
        endPosition.y = 0f;

        float distance =
            Vector3.Distance(
                currentPosition,
                endPosition);

        if (distance > linkArrivalDistance)
            return;

        CompleteCurrentOffMeshLink();
    }

    private void CompleteCurrentOffMeshLink()
    {
        waitingForPathRefresh =
            true;

        traversingOffMeshLink =
            false;

        hasJumpTarget =
            false;

        MoveDirection =
            Vector3.zero;

        jumpDirection =
            Vector3.zero;

        agent.CompleteOffMeshLink();

        agent.ResetPath();

        agent.Warp(
            transform.position);

        Invoke(
            nameof(RefreshPath),
            0.05f);
    }

    private void RefreshPath()
    {
        agent.ResetPath();

        agent.Warp(
            transform.position);

        agent.SetDestination(
            target.transform.position);

        waitingForPathRefresh =
            false;
    }

    public void SetTarget(
        Character newTarget)
    {
        target =
            newTarget;
    }

    private void OnDrawGizmos()
    {
        if (agent == null)
            return;

        if (!agent.hasPath)
            return;

        Gizmos.color =
            Color.green;

        Vector3[] corners =
            agent.path.corners;

        for (int i = 0;
             i < corners.Length - 1;
             i++)
        {
            Gizmos.DrawLine(
                corners[i],
                corners[i + 1]);
        }
    }
}