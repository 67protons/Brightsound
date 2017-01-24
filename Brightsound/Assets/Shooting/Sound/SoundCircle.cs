using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCircle : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Vector2 boostDir = this.transform.parent.GetComponent<SoundShot>().aimDirection.normalized;
            //if (player.feet.isGrounded)
            //{
            //    Debug.Log("BOOP");
            //    //player.SwitchOffGravity(0.25f);
            //}

            player.GetComponent<Rigidbody2D>().AddForce(boostDir * player.boostedEnterSpeed, ForceMode2D.Impulse);
        }
    }
}
