using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContext
{
    public Character Character { get; }

    public CharacterMotor Motor { get; }

    public CharacterAnimator Animator { get; }

    public CharacterCombat Combat { get; }

    public ICharacterBrain Brain { get; }

    public CharacterStats Stats { get; }

    public CharacterContext(
        Character character,
        CharacterMotor motor,
        CharacterAnimator animator,
        CharacterCombat combat,
        ICharacterBrain brain,
        CharacterStats stats)
    {
        Character = character;
        Motor = motor;
        Animator = animator;
        Combat = combat;
        Brain = brain;
        Stats = stats;
    }
}