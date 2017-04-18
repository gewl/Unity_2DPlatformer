using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public BoxCollider2D bodyCollider;
    public BoxCollider2D headCollider;

    protected ScoreController scoreController;
    protected Rigidbody2D rb;

    protected bool isDead = false;
    protected bool isMovingLeft = true;

    void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDead = true;

            rb.AddForce(transform.up * 500f);

            GetComponent<SpriteRenderer>().flipY = true;

            bodyCollider.isTrigger = true;
            headCollider.enabled = false;

            // Boing!
            collision.gameObject.GetComponent<PlayerController>().Jump();
            // Add to/update score.
            scoreController.increaseScore(this.transform.name);
        }
    }
}
