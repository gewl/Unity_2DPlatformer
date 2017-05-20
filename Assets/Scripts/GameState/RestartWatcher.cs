using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartWatcher : MonoBehaviour {
    // This class is attached to the GameOverDisplay, which is only made active when the player has died.
    // It watches for a spacebar press, then fires a respawn action via the GameState class.
    // It's a little silly to have this in its own script, but given that it uses an update function,
    // having it running constantly seems unnecessarily costly.

    private GameState gs;

	void Start () {
        gs = GameObject.FindObjectOfType<GameState>();
	}
	
	void Update () {
	    if (Input.GetKey(KeyCode.Space))
        {
            gs.Restart();
        }
	}
}
