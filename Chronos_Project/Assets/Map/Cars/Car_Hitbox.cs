using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //TODO kill player
        }
    }
}
