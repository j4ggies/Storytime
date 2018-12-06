using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool GamePaused = false;
    public GameObject Menu;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        Menu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    private void Pause()
    {
        Menu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }

    public void Quit() 
    {
        Application.Quit();
    }
}
