using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    private Button playButton;
    private Button quitButton;

    void Start() {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();

        playButton.onClick.AddListener(PlayOnClick);
        quitButton.onClick.AddListener(QuitOnClick);
    }

    void PlayOnClick()
    {
        SceneManager.LoadScene(1);
    }

    void QuitOnClick()
    {
        Application.Quit();
    }
}
