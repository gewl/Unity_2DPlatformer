using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from a combination of a post by LightStriker on the Unity3d forums
// and Game Programming Patterns by Robert Nystrom
public class PlayerStateMachine : ScriptableObject {

    //private object parent;
    //public object Parent { get { return parent; } }

    private GameObject player;
    public GameObject Player { get { return player; } }

    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } }

    private Rigidbody2D playerRb;
    public Rigidbody2D PlayerRb { get { return playerRb; } }

    private HealthController healthController;

    private PlayerState previousState;
    public PlayerState PreviousState { get { return previousState; } }

    private PlayerState currentState;
    public PlayerState CurrentState { get { return currentState; } }

    private PlayerState nextState;
    public PlayerState NextState { get { return nextState; } }

    private bool forced = false;

    public void Awake()
    {
        currentState = new WalkState(this);
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();
        healthController = GameObject.FindObjectOfType<HealthController>();
    }

    public void Start() { }
    
    public void AttemptToDamage()
    {
        if (IsInState(typeof(InvulnState)) == true)
        {
            return;
        }

        int currentHealth = healthController.DamagePlayer();

        if (currentHealth > 0)
        {
            ForceSwitchState(new InvulnState(this));
        } else
        {
            ForceSwitchState(new DeadState(this));
        }
    }

    public void Respawn()
    {
        ForceSwitchState(new WalkState(this));
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
            Debug.Log(currentState);

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
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
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

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Machine.PlayerController.Jump();
            Machine.SwitchState(new JumpState(Machine));
        }
    }

    public override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Machine.PlayerController.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Machine.PlayerController.MoveRight();
        }

        Vector2 vel = Machine.PlayerRb.velocity;
        if (Mathf.Abs(vel.y) > 0)
        {
            Machine.SwitchState(new JumpState(Machine));
        }
        float absoluteXVel = Mathf.Abs(vel.x);

        if (absoluteXVel > 0)
        {
            float cancelXVelocity = vel.x * -1f;
            Machine.PlayerRb.AddForce(Machine.Player.transform.right * cancelXVelocity * 5f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Machine.PlayerController.CurrentLaddersTouching > 0)
        {
            Machine.SwitchState(new LadderState(Machine));
        }

    }
}

public class JumpState : PlayerState
{
    public JumpState(PlayerStateMachine machine)
        : base(machine) { }

    private bool hasDoubleJumped;
    private int jumpTimer;

    public override void Enter()
    {
        base.Enter();

        hasDoubleJumped = false;
        jumpTimer = 20;
    }

    public override void Update()
    {
        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
        

        if (Input.GetKey(KeyCode.W) && jumpTimer == 0 && hasDoubleJumped == false)
        {
            Machine.PlayerController.Jump();
            hasDoubleJumped = true;
        }
    }

    public override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Machine.PlayerController.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Machine.PlayerController.MoveRight();
        }
        Vector2 vel = Machine.PlayerRb.velocity;
        if (vel.y == 0)
        {
            Machine.ForceSwitchState(new WalkState(Machine));
        }
        vel.y -= 50f * Time.deltaTime;
        Machine.PlayerRb.velocity = vel;

        if (Input.GetKeyDown(KeyCode.Space) && Machine.PlayerController.CurrentLaddersTouching > 0)
        {
            Machine.SwitchState(new LadderState(Machine));
        }
    }
}

public class InvulnState : PlayerState
{
    public InvulnState(PlayerStateMachine machine)
        : base(machine) { }

    private int invulnTimer;

    public override void Enter()
    {
        invulnTimer = 40;
    }

    public override void Exit()
    {
        Machine.PlayerRb.drag = 0;
    }

    public override void Update()
    {
        if (invulnTimer > 0)
        {
            invulnTimer--;
            if (invulnTimer == 0)
            {
                Machine.PlayerController.Blink();
            } else if (invulnTimer % 10 == 0)
            {
                Machine.PlayerRb.drag = 10;
                Machine.PlayerController.Blink();
            } else if (invulnTimer % 5 == 0)
            {
                Machine.PlayerController.Blink();
            }
        } else
        {
            Machine.SwitchState(new WalkState(Machine));
        }
    }

    public override void FixedUpdate()
    {
        Vector2 vel = Machine.PlayerRb.velocity;
        vel.y -= 50f * Time.deltaTime;
        Machine.PlayerRb.velocity = vel;
    }
}

public class LadderState : PlayerState
{
    public LadderState(PlayerStateMachine machine)
        : base(machine) { }

    private int leaveTimer;
    private int playerLayer;
    private int groundLayer;

    public override void Enter()
    {
        base.Enter();
        leaveTimer = 5;

        Machine.Player.transform.position = new Vector2(Machine.PlayerController.LaddersX, Machine.Player.transform.position.y);
        //Machine.PlayerRb.bodyType = RigidbodyType2D.Static;
        Machine.PlayerRb.velocity = new Vector2(0f, 0f);
        Machine.PlayerRb.gravityScale = 0f;

        Vector3 position = Machine.PlayerRb.transform.position;
        Machine.PlayerRb.transform.position = new Vector3(position.x, position.y, 2f);

        if (playerLayer == 0)
        {
            playerLayer = LayerMask.NameToLayer("Player");
            groundLayer = LayerMask.NameToLayer("Ground");
        }

        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer);
    }

    public override void Exit()
    {
        base.Exit();

        Machine.PlayerRb.gravityScale = 1f;

        Vector3 position = Machine.PlayerRb.transform.position;
        Machine.PlayerRb.transform.position = new Vector3(position.x, position.y, 0f);

        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
    }

    public override void Update()
    {
        if (leaveTimer > 0)
        {
            leaveTimer--;
        }
        if ((Input.GetKeyDown(KeyCode.Space) && leaveTimer == 0) || (Machine.PlayerController.CurrentLaddersTouching == 0))
        {
            Machine.SwitchState(new JumpState(Machine));
        }

        if (Input.GetKey(KeyCode.W))
        {
            Machine.PlayerController.MoveUp();
        } else if (Input.GetKey(KeyCode.S))
        {
            Machine.PlayerController.MoveDown();
        } else
        {
            Machine.PlayerRb.velocity = new Vector2(0f, 0f);
        }
    }
}

public class DeadState : PlayerState
{
    public DeadState(PlayerStateMachine machine)
        : base(machine) { }

    public override void Enter()
    {
        Machine.PlayerController.DieAnim();
    }

    public override void FixedUpdate()
    {
        Vector2 vel = Machine.PlayerRb.velocity;
        vel.y -= 50f * Time.deltaTime;
        Machine.PlayerRb.velocity = vel;
    }
}
