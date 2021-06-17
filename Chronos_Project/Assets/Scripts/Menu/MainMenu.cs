using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource source;
    public AudioClip click;

    private void Start()
    {
        
    }

    public void PlayGame()
    {
        source.PlayOneShot(click);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        
        
    }

    public void QuitGame()
    {
        source.PlayOneShot(click);

        Application.Quit();
    }
}
