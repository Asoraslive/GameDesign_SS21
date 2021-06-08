using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravel : MonoBehaviour
{
    //nimmt sich alle Children von Environment und setzt sie je nach Zeit auf Active
    //jedes Child von Environment braucht entweder Past oder Present Tag um Zeit zu wechseln

    private bool time; //true = past, false = present
    private Transform[] children; //alle children des Objects

    private bool traveled = false;

    private void Start()
    {
        time = false;
        children = GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !traveled) //rechtsklick ändert die Zeit
        {
            StartCoroutine("Travel");
        }
    }

    IEnumerator Travel() // Coroutine für cooldown
    {
        traveled = true;

        time = !time;
        foreach (Transform child in children)
        {
            if (child.gameObject.tag == "Past")
            {
                if (time)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
            else if (child.gameObject.tag == "Present")
            {
                if (time)
                {
                    child.gameObject.SetActive(false);
                }
                else
                {
                    child.gameObject.SetActive(true);
                }
            }
            else
            {
                Debug.Log(child.gameObject.name + "hat keine Zeit");
            }
        }
        yield return new WaitForSeconds(0.5f);

        traveled = false;
    }
}
