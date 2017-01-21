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
    Vector2 aimDir;
    float aimAngle;
    public LightShot lightShot;

    //Gets Collider for Platforms and sets drop bool
    //platform is used to switch rotational offset on platformeffector2d for downward movement then reseting it to 0
    //Drop is to see if we are going through the platform so we can account and fix the double jumping bug
    GameObject platform;
    public bool drop;

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

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            LightShot newLightShot = Instantiate(lightShot, this.transform.position, Quaternion.identity);
            newLightShot.Shoot(aimAngle);
        }
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
        if (collision.gameObject.tag == "ThroughPlatform")
        {
            if (transform.position.y < collision.transform.position.y)
                drop = true;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        platform = collision.gameObject;
    }        
}
