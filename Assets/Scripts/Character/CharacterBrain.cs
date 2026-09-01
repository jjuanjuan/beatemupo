using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour, ICharacterBrain
{
    public abstract Vector2 MoveInput { get; }
    public abstract Vector3 MoveDirection { get; }
    public abstract Vector2 LookInput { get; }
    public Vector3 JumpDirection =>
    MoveDirection;

    public bool HasJumpTarget => false;

    public Vector3 JumpTarget =>
        Vector3.zero;

    public void ConsumeJumpTarget()
    {
    }
    public abstract bool JumpPressed { get; }
    public abstract bool PunchPressed { get; }
    public abstract bool KickPressed { get; }
    public abstract bool RollPressed { get; }
    public abstract bool InteractPressed { get; }
}