using UnityEngine;

public interface ICharacterBrain
{
    Vector2 MoveInput { get; }
    Vector3 MoveDirection { get; }
    Vector2 LookInput { get; }

    Vector3 JumpDirection { get; }

    bool HasJumpTarget { get; }
    Vector3 JumpTarget { get; }

    void ConsumeJumpTarget();

    bool JumpPressed { get; }
    bool PunchPressed { get; }
    bool KickPressed { get; }
    bool RollPressed { get; }
    bool InteractPressed { get; }
}