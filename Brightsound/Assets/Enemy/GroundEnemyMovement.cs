using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMovement : MonoBehaviour {
    //Speed and direction of the enemy
    public float speed = 10f;
    Vector2 direction = Vector2.left;
    //Boundaries for enemy ai to move backwards
    public float range = 6;
    float distanceTravelled = 0;

    //Moves enemies towards direction
	void Update () {
        transform.Translate(direction * speed * Time.deltaTime);
        distanceTravelled += Mathf.Abs(direction.x * speed * Time.deltaTime);
        Debug.Log(distanceTravelled);
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
        if (direction == Vector2.left)
            direction = Vector2.right;
        else
            direction = Vector2.left;
        distanceTravelled = 0;
    }
}
