using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

    [HideInInspector]
    public bool isGrounded = false;
       
    void OnTriggerEnter2D(Collider2D other)
    {              
        if (other.CompareTag("Ground") || other.CompareTag("SolidPlatform") || other.CompareTag("ThroughPlatform"))
        {
            //This checks to see if the player went through the platform since it checks the trigger twice when going through it.
            if (transform.parent.GetComponent<Player>().drop)
            {
                transform.parent.GetComponent<Player>().drop = false;
            }
            else {
                isGrounded = true;
            }
        }        
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("SolidPlatform") || other.CompareTag("ThroughPlatform"))
        {
            isGrounded = false;
        }
    }
}
