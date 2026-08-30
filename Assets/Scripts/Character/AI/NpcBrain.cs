using UnityEngine;

public class NpcBrain : MonoBehaviour, ICharacterBrain
{
    [Header("Target")]
    [SerializeField]
    private Character target;

    [Header("Movement")]
    [SerializeField]
    private float stopDistance = 2f;

    public Vector2 MoveInput { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool PunchPressed { get; private set; }
    public bool KickPressed { get; private set; }
    public bool RollPressed { get; private set; }
    public bool InteractPressed { get; private set; }

    private void Update()
    {
        UpdateMovement();
        ResetInputs();
    }

    private void UpdateMovement()
    {
        if (target == null)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        Vector3 direction =
            target.transform.position -
            transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        if (distance <= stopDistance)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        MoveDirection =
            direction.normalized;
    }

    private void ResetInputs()
    {
        JumpPressed = false;
        PunchPressed = false;
        KickPressed = false;
        RollPressed = false;
        InteractPressed = false;
    }

    public void SetTarget(
        Character newTarget)
    {
        target = newTarget;
    }
}