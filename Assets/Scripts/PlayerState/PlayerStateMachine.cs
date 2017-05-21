using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from a combination of a post by LightStriker on the Unity3d forums
// and Game Programming Patterns by Robert Nystrom
public class PlayerStateMachine : MonoBehaviour {

    private object parent;
    public object Parent { get { return parent; } }

    private GameObject player;
    public GameObject Player { get { return player; } }

    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } }

    private PlayerState previousState;
    public PlayerState PreviousState { get { return previousState; } }

    private PlayerState currentState;
    public PlayerState CurrentState { get { return currentState; } }

    private PlayerState nextState;
    public PlayerState NextState { get { return nextState; } }

    private bool forced = false;
    
    public PlayerStateMachine (PlayerStateMachine parent)
    {
        this.parent = parent;
    }

    public PlayerStateMachine (MonoBehaviour parent)
    {
        this.parent = parent;
    }

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
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

    public void SwitchState (PlayerState nextState)
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

    public void ForceSwitchState (PlayerState nextState)
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

public abstract class PlayerState
{
    private PlayerStateMachine machine;

    public PlayerStateMachine Machine { get { return machine; } }

    protected int priority = 1;

    public int Priority { get { return priority; } }

    public PlayerState (PlayerStateMachine machine)
    {
        this.machine = machine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }
}

public class WalkState : PlayerState
{
    public WalkState(PlayerStateMachine machine) 
        : base (machine) { }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Machine.PlayerController.Jump();
        }
    }
}
