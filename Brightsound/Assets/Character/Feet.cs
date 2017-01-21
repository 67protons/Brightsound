using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour {

    //[HideInInspector]
    public bool isGrounded = false;
       
    void OnTriggerEnter2D(Collider2D other)
    {              
        if (other.CompareTag("Ground") || other.CompareTag("SolidPlatform") || other.CompareTag("ThroughPlatform"))
        {
            isGrounded = true;
            //transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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
