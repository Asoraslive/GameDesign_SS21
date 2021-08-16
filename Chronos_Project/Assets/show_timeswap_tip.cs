using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_timeswap_tip : MonoBehaviour
{
    [SerializeField] Tool_Tips tooltips;

    private void OnTriggerEnter(Collider other) {
        tooltips.Tip_timeswap(true);
    }
}
