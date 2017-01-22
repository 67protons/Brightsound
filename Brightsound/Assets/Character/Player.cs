using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    PlayerSounds playerSounds;
    SpriteRenderer sprite;

    //State
    bool invulernable = false;
    public int health = 4;

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
    [HideInInspector]
    public float lightTimer = 0f;
    public SoundShot soundShot;
    public float soundCooldown = 2f;
    [HideInInspector]
    public float soundTimer = 0f;

    public bool animLight = false;
    public bool animSound = false;


    //Gets Collider for Platforms and sets drop bool
    //platform is used to switch rotational offset on platformeffector2d for downward movement then reseting it to 0    
    GameObject platform;    

    void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.playerSounds = this.GetComponent<PlayerSounds>();
        this.sprite = this.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();
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
            //transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = new Color(255, 215, 0);
            sprite.color = new Color(255, 215, 0);
        }
        else if (!invulernable)
        {
            //transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().color = Color.white;
            sprite.color = Color.white;
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

    public void Damage(int damage)
    {
        this.health -= damage;
        StartCoroutine(BlinkRed());
        if (this.health <= 0)
        {
            Debug.Log("YOU HAVE DIED! I HAVE WRITTEN NO CODE FOR THIS YET. K-keep playing if you want");
        }
    }


    void Jump()
    {
        if (jumpCount == 0)
        {
            AudioManager.instance.PlaySFXClip(playerSounds.jumpSFX);
        }
        else if (jumpCount == 1)
        {
            AudioManager.instance.PlaySFXClip(playerSounds.doubleJumpSFX);
        }
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            if (!this.invulernable)
            {
                Enemy enemyScript = other.collider.GetComponent<Enemy>();
                Damage(enemyScript.damage);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag.Contains("Platform"))
            platform = collision.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Soundwave"))
        {
            boostDir = other.transform.parent.GetComponent<SoundShot>().aimDirection.normalized;
            Debug.Log(boostDir);
            if (feet.isGrounded)
            {
                rigidBody.gravityScale = 0f;
                Invoke("GravityOn", 0.5f);
            }
            
            rigidBody.AddForce(boostDir * boostedEnterSpeed, ForceMode2D.Impulse);
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

    void GravityOn()
    {
        rigidBody.gravityScale = 2f;
    }

    IEnumerator BlinkRed()
    {
        invulernable = true;
        float timeElapsed = 0f;

        while (timeElapsed <= 1f)
        {
            if (timeElapsed < .2f)
                sprite.color = Color.red;
            if (timeElapsed >= .2f && timeElapsed < .4f)
                sprite.color = Color.white;
            if (timeElapsed >= .4f && timeElapsed < .6f)
                sprite.color = Color.red;
            if (timeElapsed >= .6f)
                sprite.color = Color.white;

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        invulernable = false;
    }
}
