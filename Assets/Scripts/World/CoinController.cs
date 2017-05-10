using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    private ScoreController scoreController;

    void Start () {
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();
    }
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.GetComponent<CapsuleCollider2D>().isTrigger)
        {
            Destroy(gameObject);
            scoreController.increaseScore("Coin");
        }
    }
}
