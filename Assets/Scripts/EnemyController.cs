using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D bodyCollider;
    public BoxCollider2D headCollider;

    private int groundLayerId;
    private int groundLayerMask;

    private ScoreController scoreController;
    private Rigidbody2D rb;

    private bool isDead = false;
    private bool isMovingLeft = true;

	void Start () {
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();

        groundLayerId = LayerMask.NameToLayer("Ground");
        groundLayerMask = 1 << groundLayerId;

        rb = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        if (!isDead)
        {
            // TODO: rn enemies in a row will bump into each other after first reverse. they need to all reverse (include enemy in layer mask?)
            if (isMovingLeft)
            {
                RaycastHit2D[] angleHits = new RaycastHit2D[1];
                RaycastHit2D[] lateralHits = new RaycastHit2D[1];


                int angleHitsCount = bodyCollider.Raycast(new Vector2(-1.5f, -1f), angleHits, 1.5f, groundLayerMask);
                Debug.DrawLine(transform.position, angleHits[0].point);
                int lateralHitsCount = bodyCollider.Raycast(new Vector2(-1.5f, 0f), lateralHits, 0.7f);
                Debug.DrawLine(transform.position, lateralHits[0].point);

                if (angleHitsCount > 0 && lateralHitsCount == 0)
                {
                    rb.AddForce(transform.right * -10f, 0);
                }
                else
                {
                    isMovingLeft = false;
                }
            }
            else
            {
                RaycastHit2D[] angleHits = new RaycastHit2D[1];
                RaycastHit2D[] lateralHits = new RaycastHit2D[1];


                int angleHitsCount = bodyCollider.Raycast(new Vector2(1.5f, -1f), angleHits, 1.5f, groundLayerMask);
                Debug.DrawLine(transform.position, angleHits[0].point);
                int lateralHitsCount = bodyCollider.Raycast(new Vector2(1.5f, 0f), lateralHits, 0.7f);
                Debug.DrawLine(transform.position, lateralHits[0].point);

                if (angleHitsCount > 0 && lateralHitsCount == 0)
                {
                    rb.AddForce(transform.right * 10f, 0);
                }
                else
                {
                    isMovingLeft = true;
                }
            }
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 3f);

        }
        else
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

            bodyCollider.enabled = false;
            headCollider.enabled = false;

            // Boing!
            collision.gameObject.GetComponent<PlayerController>().Jump();
            // Add to/update score.
            scoreController.increaseScore("Enemy");
        }
    }
}