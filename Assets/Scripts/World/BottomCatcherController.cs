using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomCatcherController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PLayer entered");
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
