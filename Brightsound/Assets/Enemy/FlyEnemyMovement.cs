using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemyMovement : MonoBehaviour {

    //Speed and direction of the enemy
    public float speed = 10f;
    Vector2 directionX = Vector2.left;
    
    //This controls bounciness and height of flying movement sine wave stuff
    public float amplitudeY = 1f;
    float bounceFrequency = 1f;
    float bounceClock = 0f;

    //Boundaries for enemy ai to move backwards
    public float range = 6;
    float distanceTravelled = 0;

    //Moves enemies towards direction
    void Update()
    {
        //Moves enemy with sine wave as well
        transform.Translate(directionX * speed * Time.deltaTime);
        transform.localPosition = new Vector2(transform.position.x, amplitudeY * Mathf.Sin(bounceClock * bounceFrequency));
        
        //This adds to range distance and doesn't reset sine wave to 0
        float distance = Mathf.Abs(directionX.x * speed * Time.deltaTime);
        distanceTravelled += distance;
        bounceClock += distance;
        
        //Changes direction one hit max range
        if (distanceTravelled >= range)
        {
            ChangeDirection();
        }
    }

    //Changes directions when colliding with boundaries or anything
    void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }

    //Function that changes direction of enemy movement
    void ChangeDirection()
    {
        if (directionX == Vector2.left)
            directionX = Vector2.right;
        else
            directionX = Vector2.left;
        distanceTravelled = 0;
    }
}

