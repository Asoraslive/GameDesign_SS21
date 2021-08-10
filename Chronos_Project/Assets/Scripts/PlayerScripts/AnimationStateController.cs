using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody _rb;
    [SerializeField] PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", _rb.velocity.magnitude);
        animator.SetBool("inAir", PC.isFloating());
    }


    /*JumpTrigger*/
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}
