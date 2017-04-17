using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCatcherController : MonoBehaviour {

    void Start () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ding");
        Destroy(collision.gameObject);
    }
}
