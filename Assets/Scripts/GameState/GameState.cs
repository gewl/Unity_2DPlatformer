using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    GameObject player;
    GameObject gameOverDisplay;

	void Start () {
        player = GameObject.Find("Player");
        gameOverDisplay = GameObject.Find("GameOverDisplay");
        gameOverDisplay.SetActive(false);
	}

    public void GameOver ()
    {
        Debug.Log(gameOverDisplay);
        gameOverDisplay.SetActive(true);
        player.SetActive(false);
    }

    public void Restart()
    {
        gameOverDisplay.SetActive(false);
        player.SetActive(true);
        player.GetComponent<PlayerController>().Respawn();
    }
}
