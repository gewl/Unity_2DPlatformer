using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour {

    public BoxCollider2D bodyCollider;
    public BoxCollider2D headCollider;

    private int groundLayerId;
    private int groundLayerMask;

    private ScoreController scoreController;
    private Rigidbody2D rb;

    private bool isDead = false;
    private bool isMovingLeft = true;

    void Start () {
		
	}
	
	void Update () {
		
	}
}
