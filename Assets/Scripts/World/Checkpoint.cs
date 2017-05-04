using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private PlayerController pc;

	void Start () {
        pc = GameObject.FindObjectOfType<PlayerController>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pc.lastCheckpoint = this.gameObject;
        }
    }
}
