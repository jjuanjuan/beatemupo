using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContext
{
    public Character Character { get; }
    public CharacterMotor Motor { get; }
    public CharacterAnimator Animator { get; }
    public CharacterCombat Combat { get; }
    public CharacterDamage Damage { get; }
    public ICharacterBrain Brain { get; }
    public CharacterTargeting Targeting { get; }
    public CharacterInteraction Interaction { get; }
    public CharacterProjection Projection { get; }
    public CharacterStats Stats { get; }
    public CharacterStates States { get; }

    public CharacterContext(
        Character character,
        CharacterMotor motor,
        CharacterAnimator animator,
        CharacterCombat combat,
        CharacterDamage damage,
        ICharacterBrain brain,
        CharacterTargeting targeting,
        CharacterInteraction interaction,
        CharacterProjection projection,
        CharacterStats stats,
        CharacterStates states
        )
    {
        Character = character;
        Motor = motor;
        Animator = animator;
        Combat = combat;
        Damage = damage;
        Brain = brain;
        Targeting = targeting;
        Interaction = interaction;
        Projection = projection;
        Stats = stats;
        States = states;
    }
}