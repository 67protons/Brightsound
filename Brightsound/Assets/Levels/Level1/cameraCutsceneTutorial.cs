using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCutsceneTutorial : MonoBehaviour {

    bool cutscene = false;
    public Camera mainCam;
    public GameObject player;
    float zoom = 12;
    Vector3 destination = new Vector3(103, -2, -10);
    public GameObject[] triggers;

    void Start()
    {
        
    }
    void Update()
    {
        if (cutscene)
        {
            MasterGameManager.instance.inputActive = false;
            foreach (GameObject trig in triggers)
            {
                trig.SetActive(false);
            }
            player.transform.FindChild("PlayerSprite").GetComponent<Animator>().Play("PlayerIdle");
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            mainCam.GetComponent<CameraController>().target = null;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, zoom, Time.deltaTime / 2f);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, destination, Time.deltaTime / 2f);

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene = true;
        }
    }
}
