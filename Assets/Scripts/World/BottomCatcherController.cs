using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCatcherController : MonoBehaviour {

    private PlayerController pc;

    private void Start()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PLayer entered");
            pc.Respawn();
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
