using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speed = 8.0f;
    private float minSpeed = 0.05f;

    private float thrust = 14f;
    private bool isGrounded = false;

    // Ref set in Unity
    public LayerMask groundLayer;

    // Ref attained at initialize
    private Rigidbody2D rb;
    private int groundLayerId;

    // Multi-part jump.
    private int jumpStage;

    void Start () {
        // Initialize variables.
        rb = GetComponent<Rigidbody2D>();
        groundLayerId = LayerMask.NameToLayer("Ground");
    }
	
	void Update () {

        float hInput = Input.GetAxis("Horizontal") * speed;
        Vector3 vel = rb.velocity;

        if (Mathf.Abs(hInput) > minSpeed)
        {
            transform.Translate(new Vector3(hInput * Time.deltaTime, 0, 0));
        }

        // Rough jump code.
        if (Input.GetKey(KeyCode.W) && vel.y == 0f)
        {
            isGrounded = false;
            rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        }
        else if (!isGrounded)
        {
            vel.y -= 18f * Time.deltaTime;
            rb.velocity = vel;
        }

    }

    // Function to take 'current' horizontal speed and update based on player input.
    // CURRENTLY UNUSED: Reverted to "InputAxis" method.Just keeping for a commit or two in case of reversion.
    //float displacementUpdater(float speed)
    //{
    //    // Max speed of 0.2f in a given direction.
    //    if (Input.GetKey(KeyCode.D) && speed <= 0.2f)
    //    {
    //        // This code handles acceleration — fast at first, slower near upper ranges.
    //        float absSpeed = Mathf.Abs(speed);
    //        if (absSpeed >= 0.0f && absSpeed < 0.1f)
    //        {
    //            speed += 0.05f;
    //        }
    //        else if (absSpeed >= 0.1f && absSpeed < .15f)
    //        {
    //            speed += 0.02f;
    //        }
    //        else if (absSpeed >= .15f && absSpeed < 0.2f)
    //        {
    //            speed += 0.01f;
    //        }
    //        else if (speed == -0.2f)
    //        {
    //            speed += 0.01f;
    //        }
    //    }
    //    else if (Input.GetKey(KeyCode.A) && speed >= -0.2f)
    //    {
    //        float absSpeed = Mathf.Abs(speed);
    //        if (absSpeed >= 0.0f && absSpeed < 0.1f)
    //        {
    //            speed -= 0.05f;
    //        }
    //        else if (absSpeed >= 0.1f && absSpeed < .15f)
    //        {
    //            speed -= 0.02f;
    //        }
    //        else if (absSpeed >= .15f && absSpeed < 0.2f)
    //        {
    //            speed -= 0.01f;
    //        }
    //        else if (speed == 0.2f)
    //        {
    //            speed -= 0.01f;
    //        }
    //    }
    //    // Speed decays quickly if player's aren't supplying input.
    //    // Feels much better than just killing speed in absence of input.
    //    else if (!Input.GetKey(KeyCode.D) && speed > 0.0f)
    //    {
    //        speed -= 0.05f;
    //    }
    //    else if (!Input.GetKey(KeyCode.A) && speed < 0.0f)
    //    {
    //        speed += 0.05f;
    //    }

    //    //TODO: Speed values weren't staying rounded to two-decimal places, I'm not sure why.
    //    // This is an ungainly solution in the meantime — forces them to re-round at the end of each frame.
    //    speed = Mathf.Round(speed * 100f) / 100f;

    //    // Return for reassignation in update function.
    //    return speed;
    //}

    // CURRENTLY UNUSED: Switched to "velocity.y == 0" test for groundedness. Keeping code in case of reversion.
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!isGrounded && collision.gameObject.layer == groundLayerId)
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (isGrounded && collision.gameObject.layer == groundLayerId)
    //    {
    //        isGrounded = false;
    //    }
    //}
}
