using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpenTrigger : MonoBehaviour {

    public GameObject gateTrigger;
    GameObject gate;
    bool move = false;
    Vector2 distanceVector;
    GateTrigger gateScript;
    float speed;

    void Start()
    {
        gateScript = gateTrigger.GetComponent<GateTrigger>();        
        speed = gateScript.speed;
        gate = gateScript.gate;
        distanceVector = new Vector2(gate.transform.position.x, gate.transform.position.y + gateScript.distance);
    }

    void Update()
    {
        if (move)            
            gate.transform.position = Vector2.Lerp(gate.transform.position, distanceVector, Time.deltaTime * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !move)
        {
            move = true;
            gateScript.enabled = false;
        }
    }
}
