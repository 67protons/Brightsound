using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour {

    public float gateTimer;
    float timer = 0;
    public float speed = 1f;
    public GameObject gate;
    public float distance;
    [HideInInspector]
    public Vector2 distanceVector;
    Vector2 originVector;
    Vector2 padVector;
    bool gateSet = false;
    SpriteRenderer box;
    Color oldColor;

    void Start()
    {
        distanceVector = new Vector2(gate.transform.position.x, gate.transform.position.y + distance);
        originVector = gate.transform.position;
        padVector = new Vector2(originVector.x, originVector.y - 0.5f);
        box = GetComponent<SpriteRenderer>();
        oldColor = box.color;
    }

    void Update()
    {        
        if (timer > 0)
        {
            box.color = Color.white;
            gateSet = false;
            gate.transform.position = Vector2.Lerp(gate.transform.position, distanceVector, Time.deltaTime * speed);
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {            
            if (!gateSet)
                gate.transform.position = Vector2.Lerp(gate.transform.position, padVector, Time.deltaTime * speed);
            if (gate.transform.position.y - originVector.y <= 0)
            {
                box.color = oldColor;
                gate.transform.position = originVector;
                gateSet = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lightwave") && other.name == "Head")
        {            
            timer = gateTimer;
        }
    }
}
