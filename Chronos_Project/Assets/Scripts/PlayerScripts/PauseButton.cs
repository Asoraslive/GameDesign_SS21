using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject confirmWindow;

    private void Start()
    {
        Resume();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenuUI.active && !confirmWindow.active){
            Pause();

        }
        if (confirmWindow.active)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                confirmBack();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Canceled");
                cancelBack();
            }
           
        }
        else if (pauseMenuUI.active)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Resume();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                BackToMenu();
            }
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;



    }

    public void Resume()
    {
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

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
        Debug.Log("cancelBack");
        confirmWindow.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void buttonPress()
    {
        Debug.Log("Button Pressed");
    }
}
