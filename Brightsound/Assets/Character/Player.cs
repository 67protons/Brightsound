using System.Collections;
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

    //Gets Collider for Platforms
    GameObject platform;

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

        if (Input.GetKeyDown(KeyCode.S) && feet.isGrounded)
        {            
            GoThroughPlatform();            
        }
        if (platform != null && platform.tag == "ThroughPlatform" && platform.transform.position.y > transform.position.y)
        {
            platform.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
        }
        Aim();
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector3(moveDirection * speed * Time.deltaTime, 0f, 0f));        
    }

    void Jump()
    {
        StopCoroutine(Accelerate(Vector2.zero, 0f));
        StartCoroutine(Accelerate(new Vector2(0, jumpForce), jumpDuration));
    }

    IEnumerator Accelerate(Vector2 direction, float lifetime)
    {
        rigidBody.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(lifetime);
        rigidBody.velocity = Vector2.zero;
    }

    void Aim()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)this.transform.position;
        float radianAngle = Mathf.Atan2(dir.y, dir.x);
        float degreeAngle = radianAngle * Mathf.Rad2Deg;
        cursorPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, degreeAngle));
    }

    void GoThroughPlatform()
    {        
        if (platform != null)
        {
            if (platform.tag == "ThroughPlatform")
            {
                //Need to fix how to go down, rotation offset flips the effector but it doesn't go down
                transform.FindChild("Feet").GetComponent<Feet>().isGrounded = false;
                platform.GetComponent<PlatformEffector2D>().rotationalOffset = 180;                
            }            
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        platform = collision.gameObject;
    }        
}
