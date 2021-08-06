using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_links : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        this.transform.position += new Vector3(-2,0,0);
    }
}
