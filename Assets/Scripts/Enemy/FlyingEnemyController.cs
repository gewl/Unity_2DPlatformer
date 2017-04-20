using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : Enemy
{
    private Vector2 startingPos;
    private Vector3 aggroPos;

    private GameObject playerObj;

    private enum Status { Patrolling, Aggroed, Deaggroing, Bonked };
    private Status currentStatus;
    private int bonkTimer = 0;

    private bool isAggroed = false;
    private bool isDeaggroing = false;
    private int wasBonked = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreController = GameObject.Find("ScoreDisplay").GetComponent<ScoreController>();

        startingPos = transform.position;

        currentStatus = Status.Patrolling;
    }

    private void FixedUpdate()
    {
        if (!isDead && currentStatus == Status.Patrolling)
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
        else if (currentStatus == Status.Aggroed)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, 2f * Time.deltaTime);

            if (Vector3.Distance(transform.position, aggroPos) > 5f)
            {
                currentStatus = Status.Deaggroing;
            }
        }
        else if (currentStatus == Status.Deaggroing)
        {
            transform.position = Vector2.MoveTowards(transform.position, aggroPos, 2f * Time.deltaTime);
            if (transform.position == aggroPos)
            {
                currentStatus = Status.Patrolling;
            }
        }
        else if (currentStatus == Status.Bonked)
        {
            bonkTimer--;
            Vector2 vel = rb.velocity;

            rb.AddForce(new Vector2(-vel.x * 100f * Time.deltaTime, -vel.y * 100f * Time.deltaTime));

            if (bonkTimer == 0)
            {
                rb.velocity = Vector3.zero;
                currentStatus = Status.Aggroed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentStatus = Status.Bonked;
        bonkTimer = 60;
    }

    public void aggroOn (GameObject player)
    {

        playerObj = player;
        if (!isDeaggroing && !isAggroed)
        {
            aggroPos = transform.position;
        }
        currentStatus = Status.Aggroed;
    }
}
