using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour, ICharacterBrain
{
    public abstract Vector2 MoveInput { get; }

    public abstract bool JumpPressed { get; }

    public abstract bool AttackPressed { get; }
}