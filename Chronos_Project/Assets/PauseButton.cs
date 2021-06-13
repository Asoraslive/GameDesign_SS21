using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject confirmWindow;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused) { Resume(); }
            else {  Pause(); }

        }
        if (confirmWindow.active)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                confirmBack();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                cancelBack();
            }
           
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    //confirm to go back
    public void BackToMenu()
    {
        pauseMenuUI.SetActive(false);
        confirmWindow.SetActive(true);

    }

    //confirmation inside confirm window
    public void confirmBack()
    {
        SceneManager.LoadScene(0);
    }

    //cancel inside Confirm window
    public void cancelBack()
    {
        confirmWindow.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
