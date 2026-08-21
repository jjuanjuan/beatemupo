using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates
{
    public IdleState Idle { get; set; }
    public MoveState Move { get; set; }
    public JumpState Jump { get; set; }
    public FallState Fall { get; set; }
}