using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroughPlatformTrigger : MonoBehaviour {

    //public bool playerInTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB.velocity.y > 0.01f)
            {
                Physics2D.IgnoreCollision(this.transform.parent.GetComponent<Collider2D>(), other, true);
            }

        }

        //playerInTrigger = true;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(this.transform.parent.GetComponent<Collider2D>(), other, false);
        }
    }
}
