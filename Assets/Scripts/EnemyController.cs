using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private ScoreController scoreController;

    // For changing scoretext on death.
    int scoreValue = 50;

	void Start () {
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();
	}
	
	void Update () {
        transform.Translate(new Vector3(-1.5f * Time.deltaTime, 0, 0));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // TODO: Implement enemy dying animation
            Destroy(gameObject);
            // Boing!
            collision.gameObject.GetComponent<PlayerController>().Jump();
            // Add to/update score.
            scoreController.increaseScore(scoreValue);
        }
    }
}