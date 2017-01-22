using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    SpriteRenderer characterSprite;
    Transform shotCursor;
    Rigidbody2D playerRB;
    Animator animator;
    bool air = false;

    void Start()
    {
        characterSprite = GetComponent<SpriteRenderer>();
        shotCursor = transform.parent.FindChild("ShotCursor").transform;
        playerRB = transform.parent.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();        
    }

    void Update()
    {
        animator.ResetTrigger("land");
        if (shotCursor.localRotation.w >= 0.7)
        {
            characterSprite.flipX = true;
        }
        else
        {
            characterSprite.flipX = false;
        }
        
        //Backward Animation        
        if(Input.GetAxis("Horizontal") < 0 && shotCursor.localRotation.w >= 0.7 || Input.GetAxis("Horizontal") > 0 && shotCursor.localRotation.w < 0.7)
        {
            animator.SetBool("backward", true);
        }

        //Walking Animation
        else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("backward", false);
        }

        //Running Animation
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1)
        {
            animator.SetBool("run", true);          
        }
        else
        {
            animator.SetBool("run", false);
        }

        //Jumping Animation
        if (playerRB.velocity.y > 0.1f && !air)
        {
            animator.SetBool("backward", false);
            animator.SetTrigger("jump");
            air = true;
        }
        if (transform.parent.FindChild("Feet").GetComponent<Feet>().isGrounded)
        {
            animator.ResetTrigger("jump");
            animator.SetTrigger("land");
            air = false;
        }       
    }
    
}
