using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;

	void Start () {
        player = GameObject.Find("Player");
	}
	
	void Update () {
        Vector3 targetPos = new Vector3(player.transform.position.x + 5f, 0, -10);
        Vector3 zero = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref zero, .1f);
	}
}
