using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rigidBody;
    public Feet feet;

    public float speed = 5f;
    public float jumpForce = 500f;
    public float jumpDuration = 0.5f;
    private float moveDirection = 0f;

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

        //Debug.Log(rigidBody.velocity);
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
