//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Shot : MonoBehaviour {
//    public float shotDuration;
//    public float shotVelocity;

//    //public void Shoot()
//    //{

//    //}

//    //IEnumerator ShootCoroutine()
//    //{

//    //}


//}


//using UnityEngine;
//using System.Collections;

//public class Shot : MonoBehaviour
//{
//    public float MoveSpeed = 5.0f;
//    public float frequency = 20.0f;
//    public float magnitude = 0.5f;

//    private Vector3 axis;
//    private Vector3 pos;

//    void Start()
//    {
//        pos = transform.position;
//        axis = transform.right;
//    }

//    void Update()
//    {
//        pos += transform.up * Time.deltaTime * MoveSpeed;
//        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
//    }
//}

using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    TrailRenderer trailRenderer;

    float velocity = 10f;
    float frequency = 20.0f;
    float magnitude = 0.5f;

    float shotDuration = .5f;
    

    void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Shoot(shotDuration));
        }
    }

    public void SetPosition(Vector3 position)
    {

    }

    IEnumerator Shoot(float shotDuration)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= shotDuration)
        {
            Vector3 pos = trailRenderer.transform.position;
            pos += new Vector3(velocity * Time.deltaTime, 0f, 0f);
            trailRenderer.transform.position = new Vector3(pos.x, Mathf.Sin(pos.x * frequency) * magnitude, 0f);
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}