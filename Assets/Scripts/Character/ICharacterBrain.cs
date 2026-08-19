using UnityEngine;

public interface ICharacterBrain
{
    Vector2 MoveInput { get; }
    bool JumpPressed { get; }
    bool AttackPressed { get; }
}