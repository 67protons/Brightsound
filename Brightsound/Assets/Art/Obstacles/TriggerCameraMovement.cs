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

    //Speeds
    public float orthoSpeed = 6f;
    public float distSpeed = 1f;

    public GameObject exitTrigger;

    //entrance or exit?
    public bool entrance;
    bool exit;
    public bool cutscene = false;   
    
    void Start()
    {        
        mainCamScript = mainCam.GetComponent<CameraController>();
        minValDef = mainCamScript.minCameraPos;
    }
    void Update()
    {
        //if you entered the entrance
        if (enter && entrance && !exitTrigger.GetComponent<TriggerCameraMovement>().exit)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, zoom, Time.deltaTime * orthoSpeed);
            //if camera should follow
            if (follow)
            {
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z), Time.deltaTime);
                //new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z);
            }
            else {
                mainCamScript.target = null;                
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, target, Time.deltaTime * distSpeed);
            }            
        }
        
        //else if its the exit and you exited
        else if (!entrance && exit)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
            if (follow)
            {
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, new Vector3(minValDef.x, minValDef.y, minValDef.z), Time.deltaTime);
                //new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z);
            }
            else {
                mainCamScript.target = player.transform;
                if (mainCamScript.target = player.transform)
                {
                    mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
                }
            }
        }
        //If you leave the entrance
        else if (!enter && entrance)
        {
            if (follow)
            {
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, minValDef, Time.deltaTime);
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
            }
            else
            {
                mainCamScript.target = player.transform;
                if (mainCamScript.target = player.transform)
                {
                    mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
                }
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (entrance)
            {
                if (other.transform.position.x > transform.position.x)
                    enter = true;
                else
                    enter = false;
            }
            else
            {
                if (other.transform.position.x > transform.position.x)
                {
                   // enter = false;
                    exit = true;
                }
                else {
                    //enter = true;
                    exit = false;
                }
            }
        }            
    }
}
