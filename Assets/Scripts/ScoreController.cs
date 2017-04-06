using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    private Text scoreText;

    private string scoreTextPhrase = "Current score: ";
    private int currentScore;

	void Start () {
        scoreText = GetComponent<Text>();

        currentScore = 0;

        updateUIText();
	}
	
	void Update () {
		
	}

    void updateUIText ()
    {
        scoreText.text = scoreTextPhrase + currentScore;
    }

    public void increaseScore (int value)
    {
        currentScore += value;
        updateUIText();
    }
}
