using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    GameObject player;

    GameObject gameOverDisplay;
    Text pauseDisplayText;

    bool isPaused = false;

	void Start () {
        player = GameObject.Find("Player");
        gameOverDisplay = GameObject.Find("GameOverDisplay");
        gameOverDisplay.SetActive(false);

        pauseDisplayText = GameObject.Find("PauseDisplay").GetComponent<Text>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                Debug.Log("Pause");
            } else
            {
                Debug.Log("Unpause"); 
                Time.timeScale = 1;
            }
            pauseDisplayText.enabled = !pauseDisplayText.enabled;
            isPaused = !isPaused;
        }
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
