using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
