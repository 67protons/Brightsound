using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraMovement : MonoBehaviour {

    //Camera Stuff
    public Camera mainCam;
    CameraController mainCamScript;
    //Player
    public GameObject player;
    //Smoothness
    public float dampTime = 400f;
    private Vector3 velocity = Vector3.zero;
    //Public Variables to control
    public bool enter;
    public bool follow;
    Vector3 minValDef;
    //size value on camera
    public float zoom = 4;
    private float defaultZoom = 4;
    //Target position
    public Vector3 target;
    Vector3 minCamPos;
    
    void Start()
    {
        mainCamScript = mainCam.GetComponent<CameraController>();
        minValDef = mainCamScript.minCameraPos;
    }
    void Update()
    {
        if (!enter)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, zoom, Time.deltaTime);
            if (follow)
            {
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z), Time.deltaTime);
                //new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z);
            }
            else {
                mainCamScript.target = null;                
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, target, Time.deltaTime);
            }            
        }
        else if (enter)
        {
            if (follow)
            {
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, minValDef, Time.deltaTime);
            }
            if (mainCamScript.target != player.transform)
            {
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime*6f);
                Vector3 playerTemp = new Vector3(player.transform.position.x, player.transform.position.y, mainCam.transform.position.z);
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, playerTemp, Time.deltaTime * 12f);
                if (Mathf.Abs(Vector2.Distance(mainCam.transform.position, playerTemp)) < 1f)
                {
                    mainCamScript.target = player.transform;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {            
            enter = !enter;
        }            
    }
}
