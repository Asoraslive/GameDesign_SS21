using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiButton : MonoBehaviour
{
    [SerializeField] float scaleWhenClicked;
    [SerializeField] GameObject targetButton;


    public void Clicked()
    {
        if(targetButton.transform.localScale == Vector3.one)
        {
        LeanTween.scale(targetButton, new Vector3(scaleWhenClicked, scaleWhenClicked, scaleWhenClicked), .2f).setLoopPingPong(1).setIgnoreTimeScale(true);
        }
    }

    public void exitGame()
    {
        SceneManager.LoadScene(0);
    }
    
}
