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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformPoint(new Vector2(-1f, -1f)), 15f, groundLayerMask);
            Debug.DrawLine(transform.position, hit.point, Color.red);

            Debug.Log(hit.collider);

            if (hit.collider == null)
            {
                transform.Translate(new Vector3(1.5f * Time.deltaTime, 0, 0));
            }
            else
            {
                transform.Translate(new Vector3(-1.5f * Time.deltaTime, 0, 0));
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