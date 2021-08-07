using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour {
    public Vector3 direction;

    public float speed;

    private bool forward = true;

    public bool loop = false;

    private Vector3 start;
    //private Transform platform;
    private bool active = true;
    private float counter = 0.0f;
    [SerializeField]
    private float waitseconds;
    private float starttime;

    void Start() {
        start = transform.position;
        if (waitseconds != 0) {
            //Debug.Log("Warte" + waitseconds);
            active = false;
            //Debug.Log(active);
            starttime = Time.fixedTime;
        }
        //platform = this.gameObject.transform.GetChild(0);
    }


    // Update is called once per frame
    void Update() {
        if (active) {
            if (forward) {
                transform.position += (direction / speed) * Time.deltaTime;
                if ((transform.position - start).magnitude >= direction.magnitude) {
                    //Debug.Log("Umdrehen");
                    forward = false;
                    if (!loop) {
                        Destroy(this);
                    }
                }
            }
            else {
                transform.position -= (direction / speed) * Time.deltaTime;
                if ((transform.position - start).magnitude <= 0.3f) {
                    //Debug.Log("Umdrehen");
                    forward = true;
                }
            }
        }
        else {
            if (Time.fixedTime > starttime + waitseconds) {
                active = true;
            }
        }
    }
}
