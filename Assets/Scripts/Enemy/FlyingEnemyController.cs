using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : Enemy
{
    private Vector2 startingPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();

        startingPos = transform.position;

        Debug.Log(startingPos);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (isMovingLeft)
            {
                transform.position = new Vector2(transform.position.x - Time.deltaTime * 2f, transform.position.y);

                if (startingPos.x - transform.position.x > 7f)
                {
                    isMovingLeft = false;
                }
            }
            else
            {
                transform.position = new Vector2(transform.position.x + Time.deltaTime * 2f, transform.position.y);

                if (transform.position.x >= startingPos.x)
                {
                    isMovingLeft = true;
                }
            }
        }
        else
        {
            Vector2 vel = rb.velocity;
            vel.y -= 50f * Time.deltaTime;
            rb.velocity = vel;
        }
    }
}
