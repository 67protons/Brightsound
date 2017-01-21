using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour {
   
    public Vector2 destination;

    bool solid, move;
    Vector2 home;

    void Start()
    {
        //Sets if you can go through platform
        if (this.gameObject.tag == "SolidPlatform")        
            solid = true;        
        else
            solid = false;
        home = transform.position;
    }

    void Update()
    {
        if (move)
        {
            Debug.Log("same");
        }

    }
    
}
