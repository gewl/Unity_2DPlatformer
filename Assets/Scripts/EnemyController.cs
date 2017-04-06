using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D bodyCollider;
    public BoxCollider2D headCollider;

    private ScoreController scoreController;
    private Rigidbody2D rb;

    private bool isDead = false;

	void Start () {
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();

        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        if (!isDead)
        {
            transform.Translate(new Vector3(-1.5f * Time.deltaTime, 0, 0));
        } else
        {
            Vector2 vel = rb.velocity;
            vel.y -= 50f * Time.deltaTime;
            rb.velocity = vel;
        }
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDead = true;

            rb.AddForce(transform.up * 500f);

            GetComponent<SpriteRenderer>().flipY = true;

            // TODO: Implement enemy dying animation
            //Destroy(gameObject);
            bodyCollider.enabled = false;
            headCollider.enabled = false;

            // Boing!
            collision.gameObject.GetComponent<PlayerController>().Jump();
            // Add to/update score.
            scoreController.increaseScore("Enemy");
        }
    }
}