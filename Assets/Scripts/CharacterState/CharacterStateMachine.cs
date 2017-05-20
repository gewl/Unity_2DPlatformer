using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from a combination of a post by LightStriker on the Unity3d forums
// and Game Programming Patterns by Robert Nystrom
public class CharacterStateMachine : MonoBehaviour {

    private object parent;

    public object Parent { get { return parent; } }

    private State previousState;
    public State PreviousState { get { return previousState; } }

    private State currentState;
    public State CurrentState { get { return currentState; } }

    private State nextState;
    public State NextState { get { return nextState; } }

    private bool force = false;
    
    public CharacterStateMachine (CharacterStateMachine parent)
    {
        this.parent = parent;
    }
}

public abstract class State
{
      
}
