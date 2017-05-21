using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from a combination of a post by LightStriker on the Unity3d forums
// and Game Programming Patterns by Robert Nystrom
public class CharacterStateMachine : MonoBehaviour {

    private object parent;

    public object Parent { get { return parent; } }

    private CharacterState previousState;
    public CharacterState PreviousState { get { return previousState; } }

    private CharacterState currentState;
    public CharacterState CurrentState { get { return currentState; } }

    private CharacterState nextState;
    public CharacterState NextState { get { return nextState; } }

    private bool forced = false;
    
    public CharacterStateMachine (CharacterStateMachine parent)
    {
        this.parent = parent;
    }

    public CharacterStateMachine (MonoBehaviour parent)
    {
        this.parent = parent;
    }

    public void Update()
    {
        if (nextState != null)
        {
            if (currentState != null)
            {
                previousState = currentState;
                previousState.Exit();
            }

            currentState = nextState;
            currentState.Enter();

            nextState = null;

            forced = false;
        }

        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    public void SwitchState (CharacterState nextState)
    {
        if (forced)
        {
            return;
        }

        if (nextState == null || (nextState != null && this.nextState != null && nextState.Priority<this.nextState.Priority))
        {
            return;
        }

        this.nextState = nextState;
    }

    public void ForceSwitchState (CharacterState nextState)
    {
        if (forced)
        {
            return;
        }

        this.nextState = nextState;
        forced = true;
    }

    public bool IsInState (Type type)
    {
        return currentState.GetType() == type || (nextState != null && nextState.GetType() == type);
    }
}

public abstract class CharacterState
{
    private CharacterStateMachine machine;

    public CharacterStateMachine Machine { get { return machine; } }

    protected int priority = 1;

    public int Priority { get { return priority; } }

    public CharacterState (CharacterStateMachine machine)
    {
        this.machine = machine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}
