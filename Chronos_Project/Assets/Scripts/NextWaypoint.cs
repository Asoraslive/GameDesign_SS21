using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextWaypoint : MonoBehaviour
{
    [SerializeField] UnityEvent eve;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        eve.Invoke();
    }
}
