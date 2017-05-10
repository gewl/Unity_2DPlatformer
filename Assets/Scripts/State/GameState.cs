using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    GameObject god;

	void Start () {
        god = GameObject.Find("GameOverDisplay");
        Debug.Log(god);
        god.SetActive(false);
	}

    public void GameOver ()
    {
        god.SetActive(true);
    }
	
}
