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

    public Vector3 JumpDirection => jumpDirection;

    private NavMeshAgent agent;

    [Header("Target")]
    [SerializeField]
    private Character target;

    [Header("Movement")]
    [SerializeField]
    private float stopDistance = 2f;

    [Header("Jump")]
    [SerializeField] private float jumpCooldown = 0.5f;

    [Header("Obstacle Detection")]
    [SerializeField] private float obstacleCheckDistance = 0.8f;

    [SerializeField] private float lowObstacleHeight = 0.6f;
    [SerializeField] private float highObstacleCheckHeight = 1.5f;

    [SerializeField] private LayerMask climbableWalls;
    [SerializeField] private LayerMask unclimbableWalls;
    [SerializeField] private LayerMask climbableLedge;

    private float jumpCooldownTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            target = GameObject.FindWithTag("Player").GetComponent<Character>();
            return;
        }
    }

    private void Update()
    {
        JumpPressed = false;
        PunchPressed = false;
        KickPressed = false;
        RollPressed = false;
        InteractPressed = false;

        if (jumpCooldownTimer > 0f)
            jumpCooldownTimer -= Time.deltaTime;

        UpdateMovement();
        UpdateJumpDecision();
    }

    private void UpdateMovement()
    {
        agent.nextPosition =
            transform.position;

        agent.SetDestination(
            target.transform.position);

        Vector3 direction =
            agent.desiredVelocity;

        direction.y = 0f;

        if (Vector3.Distance(
                transform.position,
                target.transform.position)
            <= stopDistance)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        MoveDirection =
            direction.normalized;
    }

    public void SetTarget(
        Character newTarget)
    {
        target = newTarget;
    }

    private void UpdateJumpDecision()
    {
        Vector3 direction =
            target.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        Vector3 lowerOrigin =
            transform.position +
            Vector3.up * 0.1f;

        int obstacleMask =
            climbableWalls |
            unclimbableWalls |
            climbableLedge;

        if (!Physics.Raycast(
                lowerOrigin,
                direction,
                out RaycastHit hit,
                obstacleCheckDistance,
                obstacleMask))
        {
            return;
        }

        jumpDirection = direction;

        if (IsLowObstacle(direction))
        {
            JumpPressed = true;
            return;
        }

        CheckWallType(hit);
    }
    
    private bool IsLowObstacle(
        Vector3 direction)
    {
        Vector3 upperOrigin =
            transform.position +
            Vector3.up * lowObstacleHeight;

        int obstacleMask =
            climbableWalls |
            unclimbableWalls |
            climbableLedge;

        return !Physics.Raycast(
            upperOrigin,
            direction,
            obstacleCheckDistance,
            obstacleMask);
    }

    private void CheckWallType(
        RaycastHit hit)
    {
        int hitLayer =
            hit.collider.gameObject.layer;

        if (IsInLayerMask(
                hitLayer,
                climbableWalls))
        {
            JumpPressed = true;
            jumpCooldownTimer = jumpCooldown;
            return;
        }

        if (IsInLayerMask(
                hitLayer,
                climbableLedge))
        {
            JumpPressed = true;
            jumpCooldownTimer = jumpCooldown;
            return;
        }

        if (IsInLayerMask(
                hitLayer,
                unclimbableWalls))
        {
            return;
        }
    }

    private bool IsInLayerMask(
    int layer,
    LayerMask mask)
    {
        return
            (mask.value & (1 << layer)) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 direction =
            MoveDirection;

        if (direction.sqrMagnitude < 0.01f &&
            target != null)
        {
            direction =
                target.transform.position -
                transform.position;

            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        Vector3 lowerOrigin =
            transform.position +
            Vector3.up * 0.1f;

        Vector3 upperOrigin =
            transform.position +
            Vector3.up * lowObstacleHeight;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            lowerOrigin,
            lowerOrigin +
            direction * obstacleCheckDistance);

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            upperOrigin,
            upperOrigin +
            direction * obstacleCheckDistance);
    }
}