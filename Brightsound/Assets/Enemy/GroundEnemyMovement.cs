using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyMovement : MonoBehaviour {
    Animator animator;
    //Speed and direction of the enemy
    public float speed = 10f;
    Vector2 direction = Vector2.left;
    //Boundaries for enemy ai to move backwards
    public float range = 6;
    float distanceTravelled = 0;

    void Awake()
    {
        animator = this.transform.Find("Sprite").GetComponent<Animator>();
    }

    //Moves enemies towards direction
	void Update () {
        //This moves it
        transform.Translate(direction * speed * Time.deltaTime);
        //This adds to the range
        distanceTravelled += Mathf.Abs(direction.x * speed * Time.deltaTime);
        //Once it hits max range, it will change direction
        if (distanceTravelled >= range)
        {
            ChangeDirection();           
        }
	}

    void Attack()
    {
        animator.Play("Cello Attack");
    }

    //Changes directions when colliding with boundaries or anything
    void OnCollisionEnter2D(Collision2D collision)
    {
        //ChangeDirection();
        if (collision.collider.CompareTag("Player") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Cello Attack"))
        {
            Attack();
        }
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
