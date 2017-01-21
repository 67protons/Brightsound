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

using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
    TrailRenderer trailRenderer;

    float speed = 7.5f;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 20;

    void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
    }
    void Start()
    {
        trailRenderer.material = new Material(Shader.Find("Particles/Additive"));
        trailRenderer.startColor = c1;
        trailRenderer.endColor = c2;
    }
    void Update()
    {
        //Vector3 pos = new Vector3(speed * Time.deltaTime, Mathf.Sin(Time.deltaTime * speed * 40f), 0);
        //trailRenderer.transform.Translate(new Vector3(speed * Time.deltaTime, Mathf.Sin(trailRenderer.transform.position.x), 0f));
        trailRenderer.transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
        trailRenderer.transform.position = new Vector3(trailRenderer.transform.position.x, Mathf.Sin(trailRenderer.transform.position.x), 0f);
        
        //trailRenderer.transform.position = pos;
    }
}
