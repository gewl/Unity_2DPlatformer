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

    private void Update()
    {
        if (!isDead)
        {
            // FIX: raycast gradually 'rising,' jumps to canvas when enemy turns

            if (isMovingLeft)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformVector(new Vector2(-1.5f, -1f)), 1.5f, groundLayerMask);

                if (hit.collider != null)
                {
                    transform.Translate(new Vector3(-1.5f * Time.deltaTime, 0, 0));
                }
                else
                {
                    isMovingLeft = false;
                }

            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformVector(new Vector2(1.5f, -1f)), 1.5f, groundLayerMask);

                if (hit.collider != null)
                {
                    transform.Translate(new Vector3(1.5f * Time.deltaTime, 0, 0));
                }
                else
                {
                    isMovingLeft = true;
                }
            }


        }
    }

    void FixedUpdate () {
        if (isDead)
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