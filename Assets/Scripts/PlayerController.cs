using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Assigned to player obj during initialize.
    GameObject player;
    // Holds player's position at a given time.
    Vector3 playerPosition;
    // Keeps track of player's (horizontal) movement at a given time.
    float speed;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        speed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        // Snapshot of player's position at frame.
        playerPosition = player.transform.position;

        // Run handler function to update player obj's horizontal speed based on player's input (A & D).
        speed = speedUpdater(speed);

        // Update snapshot's x coordinates according to updated speed value.
        playerPosition.x += speed;

        // Rough jump code.
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerPosition.y += 2f;
        }

        // Reassign updated snapshot to player, yielding movement.
        player.transform.position = playerPosition;
	}

    // Function to take 'current' horizontal speed and update based on player input.
    float speedUpdater(float speed)
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
}
