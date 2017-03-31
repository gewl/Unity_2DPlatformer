using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        transform.Translate(new Vector3(-1.5f * Time.deltaTime, 0, 0));
	}
}
