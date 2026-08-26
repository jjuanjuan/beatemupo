using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour, ICharacterBrain
{
    public abstract Vector2 MoveInput { get; }
    public abstract Vector2 LookInput { get; }
    public abstract bool JumpPressed { get; }
    public abstract bool PunchPressed { get; }
    public abstract bool KickPressed { get; }
    public abstract bool RollPressed { get; }
    public abstract bool InteractPressed { get; }
}