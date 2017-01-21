using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

    //[HideInInspector]
    public bool isGrounded = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        //the transform.position.y is to not allow the player to op jump with bug
        if (other.CompareTag("Ground") || ((other.CompareTag("SolidPlatform") || other.CompareTag("ThroughPlatform")) && transform.position.y > other.transform.position.y))
        {
            isGrounded = true;
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
