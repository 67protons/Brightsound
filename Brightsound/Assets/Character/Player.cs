using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Movement
    Vector2 idleSpeed;
    Rigidbody2D rigidBody;
    public Feet feet;
    public float speed = 5f;
    public Vector2 deceleration = new Vector2(1f, 0.5f);
    public float boostedEnterSpeed = 5f;
    public Vector2 boostDir;
    public float jumpForce = 500f;
    public float jumpDuration = 0.5f;
    private float moveDirection = 0f;
    public int maxJumps = 1;
    private int jumpCount = 0;

    //Shooting
    public Transform cursorPivot;
    public Transform cursorLocation;
    Vector2 aimDir;
    float aimAngle;
    public LightShot lightShot;
    public SoundShot soundShot;

    //Gets Collider for Platforms and sets drop bool
    //platform is used to switch rotational offset on platformeffector2d for downward movement then reseting it to 0    
    GameObject platform;    

    void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if (feet.isGrounded)
            jumpCount = 0;
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
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
            platform = null;
        }
        Aim();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            LightShot newLightShot = Instantiate(lightShot, cursorLocation.position, Quaternion.identity);
            newLightShot.Shoot(aimAngle);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SoundShot newSoundShot = Instantiate(soundShot, cursorLocation.position, Quaternion.identity);
            newSoundShot.Shoot(aimAngle, aimDir);
        }
    }

    void FixedUpdate()
    {

        if (platform != null)
        {
            if (platform.tag == "SolidPlatform" || platform.tag == "ThroughPlatform")
            {
                if (platform.GetComponent<PlatformBehavior>().move /*&& rigidBody.velocity.y <= 0.01f */&& transform.FindChild("Feet").GetComponent<Feet>().isGrounded)
                {
                    idleSpeed = platform.GetComponent<PlatformBehavior>().platformVelocity;
                    //rigidBody.velocity = platform.GetComponent<PlatformBehavior>().platformVelocity;
                }               
            }
        }
        else
        {
            idleSpeed = Vector2.zero;
        }

        //Deceleration
        if (Mathf.Abs(rigidBody.velocity.x) > deceleration.x * Time.deltaTime){
            rigidBody.velocity += new Vector2(Mathf.Pow(-1, Convert.ToInt32(rigidBody.velocity.x > 0)) * deceleration.x * Time.deltaTime, 0f);
        }
        if (rigidBody.velocity.y > deceleration.y * Time.deltaTime)
        {
            rigidBody.velocity += new Vector2(0f, deceleration.y * Time.deltaTime);
        }

        transform.Translate((idleSpeed*Time.deltaTime) + new Vector2(moveDirection * speed * Time.deltaTime, 0f));
    }


    void Jump()
    {
        jumpCount++;
        StopCoroutine(Accelerate(Vector2.zero, 0f));
        rigidBody.velocity = new Vector2(0,0);
        StartCoroutine(Accelerate(new Vector2(0, jumpForce), jumpDuration));
    }

    IEnumerator Accelerate(Vector2 direction, float lifetime)
    {
        rigidBody.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(lifetime);
        //rigidBody.velocity = Vector2.zero;
    }

    void Aim()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDir = mousePos - (Vector2)this.transform.position;
        float radianAngle = Mathf.Atan2(aimDir.y, aimDir.x);
        aimAngle = radianAngle * Mathf.Rad2Deg;
        cursorPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, aimAngle));
    }

    void GoThroughPlatform()
    {        
        if (platform != null)
        {
            if (platform.tag == "ThroughPlatform")
            {
                rigidBody.velocity = new Vector2(0, 0);
                //Not grounded and change the platform effector to allow going down
                transform.FindChild("Feet").GetComponent<Feet>().isGrounded = false;
                platform.GetComponent<PlatformEffector2D>().rotationalOffset = 180;

                //This odd code will fix the down key bug and allow players to freely pass through platforms
                transform.GetComponent<BoxCollider2D>().enabled = false;
                transform.GetComponent<BoxCollider2D>().enabled = true;
            }            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("Platform"))
            platform = collision.gameObject;


    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("Platform"))
            platform = null;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soundwave"))
        {
            boostDir = other.transform.parent.GetComponent<SoundShot>().aimDirection.normalized * boostedEnterSpeed;
            rigidBody.AddForce(boostDir, ForceMode2D.Impulse);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Lightwave"))
        {
            maxJumps = 2;
        }
        else
        {
            maxJumps = 1;
        }
    }
}
