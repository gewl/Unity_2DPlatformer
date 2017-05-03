using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 vel;
    private int count = 0;

    // Constant values for movement
    private const float speed = 8.0f;
    private const float minSpeed = 0.05f;
    private const float thrust = 20f;

    // Variable values for movement
    private int currentGroundColliders = 0;
    private int jumpTimer;
    private bool hasDoubleJumped = false;

    // Variable values relating to damage
    private int invulnTimer = 0;
    private bool isDead = false;

    // Ref set in Unity
    public LayerMask groundLayer;
    public GameObject healthDisplay;
    private CapsuleCollider2D bodyCollider;

    // Ref attained at initialize
    private Rigidbody2D rb;
    private int groundLayerId;
    private HealthController healthController;
    private SpriteRenderer spriteRenderer;

    // Multi-part jump.
    private int jumpStage;

    private Animator playerAnim;

    void Start () {
        // Initialize variables.
        rb = GetComponent<Rigidbody2D>();
        groundLayerId = LayerMask.NameToLayer("Ground");
        healthController = healthDisplay.GetComponent<HealthController>();

        bodyCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerAnim = GetComponent<Animator>();
    }
	
	void FixedUpdate () {

        if (jumpTimer > 0)
        {
            jumpTimer--;
        }
        if (invulnTimer > 0)
        {
            invulnTimer--;
            if (invulnTimer == 0)
            {
                spriteRenderer.enabled = true;
            }
            else if (invulnTimer % 10 == 0)
            {
                spriteRenderer.enabled = false;
            }
            else if (invulnTimer % 5 == 0)
            {
                spriteRenderer.enabled = true;
            }
        }

        vel = rb.velocity;
        vel.y = Mathf.Floor(vel.y);

        float absoluteXVelocity = Mathf.Abs(vel.x);

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * 20f);
        } else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right * -20f);
        } else if (absoluteXVelocity > 0 && currentGroundColliders > 0)
        {
            float cancelXVelocity = vel.x * -1f;
            rb.AddForce(transform.right * cancelXVelocity * 5f);
        }

        // Rough jump code.
        if (Input.GetKey(KeyCode.W) && (currentGroundColliders > 0 || hasDoubleJumped == false) && jumpTimer == 0)
        {
            if (currentGroundColliders == 0)
            {
                hasDoubleJumped = true;
            }
            // Without this, jump input could get counted 2 or 3 times. 
            // Timer decrements 1x/frame after, giving time to escape ground colliders.
            jumpTimer = 10;
            Jump();
        }
        else if (currentGroundColliders == 0 || isDead)
        {
            vel.y -= 50f * Time.deltaTime;
            rb.velocity = vel;
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -6f, 6f), rb.velocity.y);

        playerAnim.SetFloat("Speed", absoluteXVelocity);
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

    public void Die()
    {
        rb.AddForce(transform.up * 500f);

        isDead = true;

        spriteRenderer.flipY = true;

        bodyCollider.isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGo = collision.gameObject;

        switch (collisionGo.tag)
        {
            case "Ground":
                currentGroundColliders++;
                hasDoubleJumped = false;
                break;
            // This refers to colliding with the "body" of the enemy, not the boingable head.
            case "Enemy":
                if (invulnTimer == 0) {
                    healthController.playerDamaged();
                    invulnTimer = 40;
                }

                Vector2 colliderVel = collision.rigidbody.velocity;

                // Repulses players from enemies upon being damaged—for gamefeel and to prevent repeat damage
                // This is a little rough because colliderVel references vel of incoming rigidbody AT collision,
                // not before it. Currently player bounces back in the opposite direction he'd been moving before,
                // ideally he'd move away from the object that damage him.
                float xRebound = (vel.x == 0) ? -colliderVel.x : -vel.x;
                xRebound = (xRebound > 0) ? 1f : -1f;

                float yRebound = (vel.y < 0) ? 0.5f : 0.1f;

                rb.AddForce(new Vector2(200f * xRebound, 50f * yRebound), ForceMode2D.Impulse);
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
