using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    GameObject player;
    Vector3 playerPosition;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = player.transform.position;
        
        if (Input.GetKey(KeyCode.D))
        {
            playerPosition.x += 0.1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            playerPosition.x -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            playerPosition.y += 2f;
        }

        player.transform.position = playerPosition;
	}
}
