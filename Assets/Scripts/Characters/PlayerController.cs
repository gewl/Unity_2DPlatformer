using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // For respawning
    public GameObject lastCheckpoint;

    private HealthController hc;

    private PlayerStateMachine psm;

    // Constant values for movement
    private const float speed = 30f;
    private const float thrust = 20f;
    private const float bounceBack = 500f;

    // Variable values for movement
    private Vector2 vel;
    private int currentGroundColliders = 0;
    private int jumpTimer;
    private bool hasDoubleJumped = false;

    private int currentLaddersTouching = 0;
    public int CurrentLaddersTouching { get { return currentLaddersTouching;  } }
    private float laddersX = 0f;
    public float LaddersX { get { return laddersX; } }

    // Variable values relating to damage
    public bool isDead = false;

    // Ref set in Unity
    public LayerMask groundLayer;
    private CapsuleCollider2D bodyCollider;

    // Ref attained at initialize
    private Rigidbody2D rb;
    private int groundLayerId;
    private SpriteRenderer spriteRenderer;

    // Multi-part jump.
    private int jumpStage;

    private Animator playerAnim;

    void Start () {
        // Initialize variables.
        rb = GetComponent<Rigidbody2D>();
        groundLayerId = LayerMask.NameToLayer("Ground");

        bodyCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerAnim = GetComponent<Animator>();

        psm = ScriptableObject.CreateInstance<PlayerStateMachine>();

    }

    void Update()
    {
        // delegate to finite state machine
        psm.Update();
    }

    void FixedUpdate()
    {
        psm.FixedUpdate();

        if (rb.bodyType != RigidbodyType2D.Static)
        {
            vel = rb.velocity;
            vel.y = Mathf.Floor(vel.y);

            float absoluteXVelocity = Mathf.Abs(vel.x);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -6f, 6f), rb.velocity.y);

            playerAnim.SetFloat("Speed", absoluteXVelocity);
        } else
        {
            playerAnim.SetFloat("Speed", 0);
        }
    }

    public void MoveLeft()
    {
        rb.AddForce(transform.right * -speed);
    }

    public void MoveRight()
    {
        rb.AddForce(transform.right * speed);
    }

    //for ladder movement—0 gravity on rigidbody2d is assumed
    public void MoveUp()
    {
        rb.AddForce(transform.up * speed);
    } 

    public void MoveDown()
    {
        rb.AddForce(transform.up * -speed);
    }

    // Getting ready for separating rendering & logic
    // Public so enemy logic can call on head-hit
    public void Jump()
    { 
        // Used for jumping off enemy's heads—downward momentum was
        // nullifying the bounce. This way there's a satisfying "boing"
        if (rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
    }

    public void Blink()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public void DieAnim()
    {
        rb.AddForce(transform.up * 500f);

        isDead = true;

        spriteRenderer.flipY = true;

        bodyCollider.isTrigger = true;
    }

    public void Respawn()
    {
        isDead = false;
        spriteRenderer.flipY = false;
        bodyCollider.isTrigger = false;
        currentGroundColliders = 0;

        transform.position = lastCheckpoint.transform.position;

        hc.RefreshHealth();
        psm.Respawn();
    }

    public void TouchingLadder(float x)
    {
        if (currentLaddersTouching == 0)
        {
            laddersX = x;
        }

        currentLaddersTouching++;
    }

    public void LeavingLadder()
    {
        currentLaddersTouching--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGo = collision.gameObject;

        switch (collisionGo.tag)
        {
            case "Ground":
                currentGroundColliders++;
                break;
            // This refers to colliding with the "body" of the enemy, not the boingable head.
            case "Enemy":
                psm.AttemptToDamage();

                Vector2 colliderVel = collision.rigidbody.velocity;

                // Repulses players from enemies upon being damaged—for gamefeel and to prevent repeat damage
                // This is a little rough because colliderVel references vel of incoming rigidbody AT collision,
                // not before it. Currently player bounces back in the opposite direction he'd been moving before,
                // ideally he'd move away from the object that damage him.
                float xRebound;
                if (vel.x != 0)
                {
                    xRebound = (vel.x > 0) ? -bounceBack : bounceBack;
                } else
                {
                    xRebound = (colliderVel.x < 0) ? -bounceBack : bounceBack;
                }

                float yRebound = (vel.y < 0) ? 0.5f : 0.1f;

                rb.AddForce(new Vector2(xRebound, 50f * yRebound), ForceMode2D.Impulse);
                break;
            default:
                Debug.Log(collisionGo.tag);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    { 
        GameObject collisionGo = collision.gameObject;

        if (collisionGo.tag == "Ground")
        {
            currentGroundColliders--;
        } 
    }
}
