using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 vel;

    private float speed = 8.0f;
    private float minSpeed = 0.05f;

    private float thrust = 20f;
    private int currentGroundColliders = 0;

    // Ref set in Unity
    public LayerMask groundLayer;
    public GameObject healthDisplay;
    private CapsuleCollider2D bodyCollider;

    // Ref attained at initialize
    private Rigidbody2D rb;
    private int groundLayerId;
    private HealthController healthController;

    // Multi-part jump.
    private int jumpStage;

    private Animator playerAnim;

    void Start () {
        // Initialize variables.
        rb = GetComponent<Rigidbody2D>();
        groundLayerId = LayerMask.NameToLayer("Ground");
        healthController = healthDisplay.GetComponent<HealthController>();

        bodyCollider = GetComponent<CapsuleCollider2D>();

        playerAnim = GetComponent<Animator>();
    }
	
	void FixedUpdate () {

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
        if (Input.GetKey(KeyCode.W) && vel.y == 0f)
        {
            Jump();
        }
        else if (currentGroundColliders == 0)
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

        GetComponent<SpriteRenderer>().flipY = true;

        bodyCollider.enabled = false;
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
                healthController.playerDamaged();

                Vector2 colliderVel = collision.rigidbody.velocity;

                // Repulses players from enemies upon being damaged—for gamefeel and to prevent repeat damage
                // This is a little rough because colliderVel references vel of incoming rigidbody AT collision,
                // not before it. Currently player bounces back in the opposite direction he'd been moving before,
                // ideally he'd move away from the object that damage him.
                float xRebound = (vel.x == 0) ? -colliderVel.x : -vel.x;
                xRebound = (xRebound > 0) ? 1f : -1f;

                float yRebound = (colliderVel.y < 0) ? -0.1f : 0.1f;

                rb.AddForce(new Vector2(100f * xRebound, 50f * yRebound), ForceMode2D.Impulse);
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
