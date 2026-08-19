using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    public CharacterState CurrentState { get; private set; }

    public void ChangeState(CharacterState newState)
    {
        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}