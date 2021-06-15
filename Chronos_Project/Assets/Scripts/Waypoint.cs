using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Text dis;
    public Camera cam;
    private Transform[] targets;
    private int i = 1;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targets = GetComponentsInChildren<Transform>();

        //Grenzwerte für den Waypoint auf Canvas
        float minX = dis.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = dis.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.width - minY;

        Vector2 pos = cam.WorldToScreenPoint(targets[i].position); //Wandelt World Informationen in 2D Informationen um

        //Wenn Waypoint hinter der Kamera ist, wird er an den Rand des Screens gesetzt
        if (Vector3.Dot((targets[i].position - cam.transform.position), cam.transform.forward) < 0)
        {
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        dis.transform.position = pos;
        dis.text = ((int)Vector3.Distance(targets[i].position, cam.transform.position)).ToString() + "m";
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hier");
        if (other.CompareTag("Player"))
        {
            i++;
        }
    }
}
