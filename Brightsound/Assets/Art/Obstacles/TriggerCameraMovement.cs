using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraMovement : MonoBehaviour {

    //Camera Stuff
    Camera mainCam;
    CameraController mainCamScript;

    //Player
    public GameObject player;

    //Smoothness
    public float dampTime = 400f;
    private Vector3 velocity = Vector3.zero;

    //Public Variables to control
    public bool hasEntered;
    public bool follow;
    public bool isRefocussing;
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

    public GameObject pairedTrigger;

    //Which side?
    public bool isLeftTrigger;
    bool isRightTrigger;

    public bool cutscene = false;   
    
    void Start()
    {     
        mainCam = Camera.main;   
        mainCamScript = mainCam.GetComponent<CameraController>();
        minValDef = mainCamScript.minCameraPos;
        if (pairedTrigger != null)
        {
            isLeftTrigger = this.transform.position.x < pairedTrigger.transform.position.x;
            isRightTrigger = !isLeftTrigger;
            //Check if player starts between triggers
            if (isLeftTrigger)
            {
                hasEntered = (player.transform.position.x > transform.position.x) && (player.transform.position.x < pairedTrigger.transform.position.x);
                pairedTrigger.GetComponent<TriggerCameraMovement>().hasEntered = hasEntered;
            }
        } else
        {
            hasEntered = false;
        }
    }
    void Update()
    {
        //Make sure the camera only moves when we want it to
        if (isRefocussing)
        {
            if (hasEntered)
            {
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, zoom, Time.deltaTime * orthoSpeed);
                //if camera should follow
                if (follow)
                {
                    mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z), Time.deltaTime);
                    if ((mainCamScript.minCameraPos - new Vector3(minValDef.x, minValDef.y + (zoom - defaultZoom), minValDef.z)).magnitude <= 0.1f)
                        isRefocussing = false;
                }
                else
                {
                    mainCamScript.target = null;
                    mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, target, Time.deltaTime * distSpeed);
                    if ((mainCam.transform.position - target).magnitude <= 0.1f)
                        isRefocussing = false;
                }
            }
            else
            {
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
                mainCamScript.minCameraPos = Vector3.Lerp(mainCamScript.minCameraPos, new Vector3(minValDef.x, minValDef.y, minValDef.z), Time.deltaTime);
                if (follow)
                {
                    if ((mainCamScript.minCameraPos - new Vector3(minValDef.x, minValDef.y, minValDef.z)).magnitude <= 0.1f)
                        isRefocussing = false;
                }
                else
                {
                    mainCamScript.target = player.transform;
                    mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, defaultZoom, Time.deltaTime * orthoSpeed);
                    if (mainCam.orthographicSize - defaultZoom <= 0.1f)
                        isRefocussing = false;
                }
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Check if we're between the triggers
            hasEntered = (other.transform.position.x > transform.position.x) == isLeftTrigger;
            isRefocussing = true;
            if (pairedTrigger != null)
            {
                //Ensure that the paired trigger agrees with the first trigger we entered
                pairedTrigger.GetComponent<TriggerCameraMovement>().hasEntered = hasEntered;
                pairedTrigger.GetComponent<TriggerCameraMovement>().follow = follow;
                pairedTrigger.GetComponent<TriggerCameraMovement>().target = target;
                pairedTrigger.GetComponent<TriggerCameraMovement>().zoom = zoom;
                pairedTrigger.GetComponent<TriggerCameraMovement>().orthoSpeed = orthoSpeed;
                pairedTrigger.GetComponent<TriggerCameraMovement>().distSpeed = distSpeed;
            }
        }            
    }
}
