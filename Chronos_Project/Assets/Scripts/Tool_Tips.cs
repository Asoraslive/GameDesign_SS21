using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Tips : MonoBehaviour
{
    [SerializeField] GameObject tip_wasd;
    [SerializeField] GameObject tip_space;
    [SerializeField] GameObject tip_wallride;
    [SerializeField] GameObject tip_e;
    [SerializeField] GameObject tip_doublejump;
    [SerializeField] GameObject tip_timeswap;

    private bool act_wasd = false;
    private bool act_space = false;
    private bool act_wwallride = false;
    private bool act_doublejump = false;
    private bool act_timeswap = false;

    private bool pressed = false;


    private void Start() {
        Tip_wasd(true);
    }

    private void Update() {
        if (tip_wasd.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.W)) {
                pressed = true;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                pressed = true;
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                pressed = true;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                pressed = true;
            }
            if (pressed) {
                Tip_wasd(false);
            }
        }

        if (tip_space.activeInHierarchy) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Tip_space(false);
            }
        }

        if (tip_timeswap.activeInHierarchy) {
            if (Input.GetMouseButtonDown(1)) {
                Tip_timeswap(false);
            }
        }
    }

    public void Tip_wasd(bool activate) {
        if (activate && !act_wasd) {
            tip_wasd.SetActive(true);
            act_wasd = true;
        }
        else {
            tip_wasd.SetActive(false);
            Tip_space(true);
        }
    }

    public void Tip_space(bool activate) {
        if (activate && !act_space) {
            tip_space.SetActive(true);
            act_space = true;
        }
        else {
            tip_space.SetActive(false);
            Tip_doublejump(true);
        }
    }

    public void Tip_doublejump(bool activate) {
        if (activate && !act_doublejump) {
            tip_doublejump.SetActive(true);
            act_doublejump = true;
        }
        else {
            tip_doublejump.SetActive(false);
            Tip_wallride(true);
        }
    }

    public void Tip_wallride(bool activate) {
        if (activate && !act_wwallride) {
            tip_wallride.SetActive(true);
            act_wwallride = true;
        }
        else {
            tip_wallride.SetActive(false);
        }
    }

    public void Tip_e(bool activate) {
        if (activate) {
            tip_e.SetActive(true);
        }
        else {
            tip_e.SetActive(false);
        }
    }

    public void Tip_timeswap(bool activate) {
        if (activate && !act_timeswap) {
            tip_timeswap.SetActive(true);
            act_timeswap = true;
        }
        else if(!activate) {
            tip_timeswap.SetActive(false);
        }
    }

}
