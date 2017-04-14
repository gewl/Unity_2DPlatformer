using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	void Update () {
        transform.position = new Vector3(player.transform.position.x + 10f, 0, -10);
	}
}
