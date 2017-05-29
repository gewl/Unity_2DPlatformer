using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {
    // This class is attached to the GameOverDisplay, which is only made active when the player has died.
    // It watches for a spacebar press, then fires a respawn action via the GameState class.
    // It's a little silly to have this in its own script, but given that it uses an update function,
    // having it running constantly seems unnecessarily costly.

    private GameState gs;
    private Text primaryText;
    private GameObject childTextObject;

	void Awake () {
        gs = GameObject.FindObjectOfType<GameState>();
        primaryText = GetComponent<Text>();
        childTextObject = this.transform.GetChild(0).gameObject;
	}
	
	void Update () {
	    if (Input.GetKey(KeyCode.Space))
        {
            gs.Restart();
        }
	}

    private void OnEnable()
    {
        primaryText.enabled = true;
        childTextObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        primaryText.enabled = false; 
    }
}
