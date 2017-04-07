using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speed = 8.0f;
    private float minSpeed = 0.05f;

    private float thrust = 20f;
    private int currentGroundColliders = 0;

    // Ref set in Unity
    public LayerMask groundLayer;
    public GameObject healthDisplay;

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

        playerAnim = GetComponent<Animator>();
    }
	
	void FixedUpdate () {

        Vector2 vel = rb.velocity;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGo = collision.gameObject;

        switch (collisionGo.tag)
        {
            case "Ground":
                currentGroundColliders++;
                break;
            // TODO: Implement damage
            // This refers to the "body" of the enemy, not the boingable head.
            case "Enemy":
                Debug.Log("ouch!");
                healthController.playerDamaged();
                break;
            default:
                Debug.Log(collisionGo.tag);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    { 
        GameObject collisionGo = collision.gameObject;

        if (collisionGo.layer == groundLayerId)
        {
            currentGroundColliders--;
        }
    }
}
