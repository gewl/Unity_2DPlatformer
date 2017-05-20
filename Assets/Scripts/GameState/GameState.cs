using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    GameObject god;
    GameObject player;

	void Start () {
        god = GameObject.Find("GameOverDisplay");
        god.SetActive(false);

        player = GameObject.Find("Player");
	}

    public void GameOver ()
    {
        god.SetActive(true);
        player.SetActive(false);
    }

    public void Restart()
    {
        god.SetActive(false);
        player.SetActive(true);
        player.GetComponent<PlayerController>().Respawn();
    }
}
