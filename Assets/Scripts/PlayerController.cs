using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Holds player's position at a given time.
    Vector3 playerPosition;
    // Keeps track of player's (horizontal) movement at a given time.
    float displacement;
    float speed = 50.0f;

    // Reference to objects in 'ground' layer
    public LayerMask groundLayer;

    //Adjustable variable for jump height
    private float jumpHeight = 3f;

    // Ref attained at initialize
    private Rigidbody2D rigidBody;
    // Checks if a player is grounded
    bool isGrounded = false;
    // Get Layer ID for groundLayer;
    int groundLayerId;

    // Use this for initialization
    void Start () {
        // Initialize variables.
        rigidBody = GetComponent<Rigidbody2D>();
        groundLayerId = LayerMask.NameToLayer("Ground");
    }
	
	// Update is called once per frame
	void Update () {

        // Run handler function to update player obj's horizontal speed based on player's input (A & D).
        displacement = displacementUpdater(displacement);

        // Update snapshot's x coordinates according to updated speed value.
        transform.Translate(new Vector3(displacement * Time.deltaTime * speed, 0f, 0f));

        // Rough jump code.
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            transform.Translate(new Vector3(0f, jumpHeight, 0f));
        }
        
    }

    // Function to take 'current' horizontal speed and update based on player input.
    float displacementUpdater(float speed)
    {
        // Max speed of 0.2f in a given direction.
        if (Input.GetKey(KeyCode.D) && speed <= 0.2f)
        {
            // This code handles acceleration — fast at first, slower near upper ranges.
            float absSpeed = Mathf.Abs(speed);
            if (absSpeed >= 0.0f && absSpeed < 0.1f)
            {
                speed += 0.05f;
            }
            else if (absSpeed >= 0.1f && absSpeed < .15f)
            {
                speed += 0.02f;
            }
            else if (absSpeed >= .15f && absSpeed < 0.2f)
            {
                speed += 0.01f;
            }
            else if (speed == -0.2f)
            {
                speed += 0.01f;
            }
        }
        else if (Input.GetKey(KeyCode.A) && speed >= -0.2f)
        {
            float absSpeed = Mathf.Abs(speed);
            if (absSpeed >= 0.0f && absSpeed < 0.1f)
            {
                speed -= 0.05f;
            }
            else if (absSpeed >= 0.1f && absSpeed < .15f)
            {
                speed -= 0.02f;
            }
            else if (absSpeed >= .15f && absSpeed < 0.2f)
            {
                speed -= 0.01f;
            }
            else if (speed == 0.2f)
            {
                speed -= 0.01f;
            }
        }
        // Speed decays quickly if player's aren't supplying input.
        // Feels much better than just killing speed in absence of input.
        else if (!Input.GetKey(KeyCode.D) && speed > 0.0f)
        {
            speed -= 0.05f;
        }
        else if (!Input.GetKey(KeyCode.A) && speed < 0.0f)
        {
            speed += 0.05f;
        }

        //TODO: Speed values weren't staying rounded to two-decimal places, I'm not sure why.
        // This is an ungainly solution in the meantime — forces them to re-round at the end of each frame.
        speed = Mathf.Round(speed * 100f) / 100f;

        // Return for reassignation in update function.
        return speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.layer == groundLayerId)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isGrounded && collision.gameObject.layer == groundLayerId)
        {
            isGrounded = false;
        }
    }
}
