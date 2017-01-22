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
    Collider2D lightwave = null;

    //Shooting
    public Transform cursorPivot;
    public Transform cursorLocation;
    Vector2 aimDir;
    public float aimAngle;
    public LightShot lightShot;
    public float lightCooldown = 2f;
    private float lightTimer = 0f;
    public SoundShot soundShot;
    public float soundCooldown = 2f;
    private float soundTimer = 0f;

    public bool animLight = false;
    public bool animSound = false;


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

        if(Input.GetKeyDown(KeyCode.Mouse0) && lightTimer <= 0)
        {
            LightShot newLightShot = Instantiate(lightShot, cursorLocation.position, Quaternion.identity);
            newLightShot.Shoot(aimAngle);
            lightTimer = lightCooldown;
            animLight = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && soundTimer <= 0)
        {
            SoundShot newSoundShot = Instantiate(soundShot, cursorLocation.position, Quaternion.identity);
            newSoundShot.Shoot(aimAngle, aimDir);
            soundTimer = soundCooldown;
            animSound = true;
        }

        if (platform != null)
        {
            if (platform.tag == "SolidPlatform" || platform.tag == "ThroughPlatform")
            {
                if (platform.GetComponent<PlatformBehavior>().move && transform.FindChild("Feet").GetComponent<Feet>().isGrounded)
                {
                    idleSpeed = platform.GetComponent<PlatformBehavior>().platformVelocity;
                }
            }
        }
        else
        {
            idleSpeed = Vector2.zero;
        }

        ManageState();
    }

    void FixedUpdate()
    {
        transform.Translate((idleSpeed*Time.deltaTime) + new Vector2(moveDirection * speed * Time.deltaTime, 0f));
    }

    void ManageState()
    {
        Aim();

        //Ability cooldowns
        if (lightTimer > 0)
            lightTimer -= Time.deltaTime;
        if (soundTimer > 0)
            soundTimer -= Time.deltaTime;

        //Check if a lightwave we touched died before we exited it
        if (lightwave == null)
        {
            maxJumps = 1;
        }

        if (maxJumps == 2)
        {
            transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = new Color(255, 215, 0);
        }
        else
        {
            transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = Color.white;
        }

        //Deceleration
        if (Mathf.Abs(rigidBody.velocity.x) > deceleration.x * Time.deltaTime)
        {
            rigidBody.velocity += new Vector2(Mathf.Pow(-1, Convert.ToInt32(rigidBody.velocity.x > 0)) * deceleration.x * Time.deltaTime, 0f);
        }
        if (rigidBody.velocity.y > deceleration.y * Time.deltaTime)
        {
            rigidBody.velocity += new Vector2(0f, deceleration.y * Time.deltaTime);
        }
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
                //transform.GetComponent<BoxCollider2D>().enabled = false;
                //transform.GetComponent<BoxCollider2D>().enabled = true;
            }            
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("Platform"))
            platform = collision.gameObject;


    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.collider.tag.Contains("Platform"))
        //    platform = null;
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
            this.lightwave = other;
            maxJumps = 2;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lightwave"))
        {
            maxJumps = 1;
        }
    }
}
