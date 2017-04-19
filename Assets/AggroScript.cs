using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroScript : MonoBehaviour {

    FlyingEnemyController fec;

	void Start () {
        fec = this.transform.GetComponentInParent<FlyingEnemyController>();
	}
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fec.aggroOn(collision.gameObject);
        }
    }
}
