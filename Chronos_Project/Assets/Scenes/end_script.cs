using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class end_script : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
}
