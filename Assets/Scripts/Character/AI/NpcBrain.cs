using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBrain : MonoBehaviour, ICharacterBrain
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }

    public bool PunchPressed { get; private set; }
    public bool KickPressed { get; private set; }
}
