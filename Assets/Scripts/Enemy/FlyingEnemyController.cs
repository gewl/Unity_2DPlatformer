using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : Enemy
{
    private Vector2 startingPos;
    private Vector3 aggroPos;

    private GameObject playerObj;

    private bool isAggroed = false;
    private bool isDeaggroing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();

        startingPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isDead && !isAggroed && !isDeaggroing)
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
        else if (isDead)
        {
            Vector2 vel = rb.velocity;
            vel.y -= 50f * Time.deltaTime;
            rb.velocity = vel;
        }
        else if (isAggroed)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, 2f * Time.deltaTime);

            if (Vector3.Distance(transform.position, aggroPos) > 5f)
            {
                isAggroed = false;
                isDeaggroing = true;
            }
        }
        else if (isDeaggroing)
        {
            transform.position = Vector2.MoveTowards(transform.position, aggroPos, 2f * Time.deltaTime);
            if (transform.position == aggroPos)
            {
                isDeaggroing = false;
            }
        }
    }

    public void aggroOn (GameObject player)
    {
        Debug.Log("aggroed");

        playerObj = player;
        if (!isDeaggroing && !isAggroed)
        {
            aggroPos = transform.position;
        }
        isAggroed = true;
    }
}
