using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour {
   
    public Vector2 destination;

    public bool move;
    Vector2 home;
    float timeIncrement = 0;

    float bounceFrequency = 1f;
    float bounceClock = 0f;

    public float smoothTime = 0.3f;
    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    public Vector2 platformVelocity;

    void Start()
    {        
        home = transform.position;
    }

    void Update()
    {
        if (move)
        {            
            float newPositionX = Mathf.SmoothDamp(transform.position.x, destination.x, ref xVelocity, smoothTime);
            float newPositionY = Mathf.SmoothDamp(transform.position.y, destination.y, ref yVelocity, smoothTime);
            platformVelocity = ((new Vector3(newPositionX, newPositionY, 0) - transform.position) / Time.deltaTime);
            transform.position = new Vector2(newPositionX, newPositionY);            
            if (Mathf.Abs(destination.x - transform.position.x) < 0.05f && Mathf.Abs(destination.y - transform.position.y) < 0.05f)
            {
                transform.position = destination;
                Vector2 temp = home;
                home = destination;
                destination = temp;
            }

            //timeIncrement += Time.deltaTime;
            //transform.position = Vector2.Lerp(home, destination, timeIncrement * speed);
            //if (timeIncrement*speed >= 1)
            //{
            //    timeIncrement = 0;
            //    Vector2 temp = home;
            //    home = destination;
            //    destination = temp;
            //}            
            //Vector2 direction = destination - home;
            //direction.Normalize();
            //transform.Translate(direction * speed * Time.deltaTime * Mathf.Sin(bounceClock * bounceFrequency));
        }

    }  
}
