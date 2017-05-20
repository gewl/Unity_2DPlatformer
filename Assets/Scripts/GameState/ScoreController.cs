using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    private Text scoreText;

    private string scoreTextPhrase = "Current score: ";
    private int currentScore;

    // Dict to hold values of different score-incrementers (enemies, coins, etc.) in one centralized place for easy tweaking.
    // Initialized by running 'initializeDict' function on start.
    Dictionary<string, int> valueDict;

	void Start () {
        scoreText = GetComponent<Text>();

        currentScore = 0;

        updateUIText();
        initializeDict();
	}
	
	void Update () {
		
	}

    void initializeDict()
    {
        valueDict = new Dictionary<string, int>();
        valueDict.Add("Coin", 100);
        valueDict.Add("GroundEnemy", 50);
        valueDict.Add("FlyingEnemy", 150);
    }

    void updateUIText ()
    {
        scoreText.text = scoreTextPhrase + currentScore;
    }

    public void increaseScore (string name)
    {
        int value = valueDict[name];
        currentScore += value;
        updateUIText();
    }
}
