using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates
{
    public IdleState Idle { get; set; }
    public MoveState Move { get; set; }
    public JumpState Jump { get; set; }
    public FallState Fall { get; set; }
    public LandingState Landing { get; set; }
    public AttackState Attack { get; set; }
    public HitState Hit { get; set; }
    public KnockdownState Knockdown { get; set; }
    public DeathState Death { get; set; }
    public LedgeHangState LedgeHang { get; set; }
    public LedgeClimbState LedgeClimb { get; set; }
}