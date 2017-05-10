using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCatcherController : MonoBehaviour {

    private PlayerController pc;
    private GameState gs;

    private void Start()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
        gs = GameObject.FindObjectOfType<GameState>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player entered");
            gs.GameOver();
            //pc.Respawn();
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
