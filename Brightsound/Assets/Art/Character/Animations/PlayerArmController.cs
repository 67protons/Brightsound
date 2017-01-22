using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmController : MonoBehaviour {

    Animator animator;
    Transform shotCursor;
    SpriteRenderer armSprite;
    Player playerScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        shotCursor = transform.parent.transform.parent.FindChild("ShotCursor").transform;
        armSprite = GetComponent<SpriteRenderer>();
        playerScript = transform.parent.transform.parent.GetComponent<Player>();
    }

    void Update () {
        float aimAngle = transform.parent.transform.parent.GetComponent<Player>().aimAngle;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IdleState"))
        {
            transform.rotation = (Quaternion.identity);
            armSprite.flipY = false;
        }

        if (playerScript.animArmLight)
        {
            transform.rotation = (Quaternion.identity);
            this.transform.Rotate(0f, 0f, aimAngle);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerArmShot"))
                animator.SetTrigger("shoot");
            playerScript.animArmLight = false;
        }
        if (playerScript.animArmSound)
        {
            transform.rotation = (Quaternion.identity);
            this.transform.Rotate(0f, 0f, aimAngle);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerArmShot"))
                animator.SetTrigger("shoot");
            playerScript.animArmSound = false;
        }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerArmShot") && shotCursor.localRotation.w < 0.7f)
        {
            armSprite.flipX = true;
            armSprite.flipY = true;
        }
        else if (shotCursor.localRotation.w >= 0.7)
        {
            armSprite.flipX = true;
        }
        else
        {
            armSprite.flipX = false;
        }
    }
}
