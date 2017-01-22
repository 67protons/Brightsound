using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    public Sprite shoot;
    Sprite idle;
    SpriteRenderer characterSprite;
    Transform shotCursor;
    Rigidbody2D playerRB;
    Animator animator;
    Player playerScript;
    bool air = false;    

    void Start()
    {
        characterSprite = GetComponent<SpriteRenderer>();
        shotCursor = transform.parent.FindChild("ShotCursor").transform;
        playerRB = transform.parent.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        idle = GetComponent<SpriteRenderer>().sprite;
        playerScript = transform.parent.GetComponent<Player>();
    }

    void Update()
    {        
        if (playerScript.animLight)
        {
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJump") || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpIdle")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpShot"))
                animator.SetTrigger("jumpShot");
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerBodyShot") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpShot") && !air)
            {
                animator.ResetTrigger("shoot");
                animator.SetTrigger("shoot");
            }
            playerScript.animLight = false;
        }        
        if (playerScript.animSound)
        {
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJump") || animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpIdle")) && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpShot"))
                animator.SetTrigger("jumpShot");
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerBodyShot") && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJumpShot") && !air)
            {
                animator.ResetTrigger("shoot");
                animator.SetTrigger("shoot");
            }
            playerScript.animSound = false;
        }
        
        //Resets on landing
        animator.ResetTrigger("land");
                
        //Sets flipping of sprite
        if (shotCursor.localRotation.w >= 0.7)
        {
            characterSprite.flipX = true;
        }
        else
        {
            characterSprite.flipX = false;
        }
        
        //Backward Animation        
        if((Input.GetAxis("Horizontal") < 0 && shotCursor.localRotation.w >= 0.7) || (Input.GetAxis("Horizontal") > 0 && shotCursor.localRotation.w < 0.7))
        {
            animator.SetBool("backward", true);
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
        }

        //Running Animation
        else if ((Input.GetAxis("Horizontal") >= 1 && shotCursor.localRotation.w >= 0.7) || (Input.GetAxis("Horizontal") <= -1) && shotCursor.localRotation.w < 0.7)
        {
            animator.SetBool("walk", true);
            animator.SetBool("run", true);            
            animator.SetBool("backward", false);
        }

        //Walking Animation
        else if ((Input.GetAxis("Horizontal") > 0 && shotCursor.localRotation.w >= 0.7) || (Input.GetAxis("Horizontal") < 0) && shotCursor.localRotation.w < 0.7)
        {
            animator.SetBool("walk", true);
            animator.SetBool("backward", false);
        }


        //Reset if none of the above
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("backward", false);
            animator.SetBool("run", false);
        }


        //Jumping Animation
        if (playerRB.velocity.y > 0.01f && !air)
        {
            animator.SetBool("backward", false);
            animator.SetTrigger("jump");
            air = true;
        }
        if (transform.parent.FindChild("Feet").GetComponent<Feet>().isGrounded)
        {
            animator.ResetTrigger("jump");
            animator.ResetTrigger("shoot");
            animator.SetTrigger("land");
            air = false;
        }       
    }
    
}
