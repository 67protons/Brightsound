﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Movement
    Rigidbody2D rigidBody;
    public Feet feet;
    public float speed = 5f;
    public float jumpForce = 500f;
    public float jumpDuration = 0.5f;
    private float moveDirection = 0f;

    //Shooting
    public Transform cursorPivot;

    void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && feet.isGrounded)
        {
            Jump();
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)this.transform.position;
        float radianAngle = Mathf.Atan2(dir.y, dir.x);
        float degreeAngle = radianAngle * Mathf.Rad2Deg;
        cursorPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, degreeAngle));
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector3(moveDirection * speed * Time.deltaTime, 0f, 0f));
    }

    void Jump()
    {
        //Vector2 force = (transform.up * jumpForce) / Time.fixedDeltaTime;
        StopCoroutine(Accelerate(0f));
        StartCoroutine(Accelerate(jumpDuration));
        
    }

    IEnumerator Accelerate(float lifetime)
    {
        rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(lifetime);
        rigidBody.velocity = Vector2.zero;
    }
}
