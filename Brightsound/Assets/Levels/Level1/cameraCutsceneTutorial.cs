using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCutsceneTutorial : MonoBehaviour
{

    bool cutscene = false;
    public Camera mainCam;
    public GameObject player;
    public SpriteRenderer title;
    float zoom = 12;
    Vector3 destination = new Vector3(103, -2, -10);
    public GameObject[] triggers;
    bool checkForKick = false;

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
            player.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>().flipX = true;
            //player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Player>().moveDirection = 0f;
            mainCam.GetComponent<CameraController>().target = null;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, zoom, Time.deltaTime / 2f);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, destination, Time.deltaTime / 2f);

            checkForKick = true;
        }

        if (checkForKick && (mainCam.transform.position.x >= destination.x - 0.1f && mainCam.transform.position.x <= destination.x + 0.1f))
        {
            cutscene = false;
            StartCoroutine(FadeTitle());

            player.GetComponent<Player>().moveDirection = 1f;
            player.GetComponent<Player>().Jump(false);

            checkForKick = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cutscene = true;
        }
    }

    IEnumerator FadeTitle()
    {
        float timeElapsed = 0f;

        while (title.color.a > 0.01f)
        {
            timeElapsed += Time.deltaTime;
            title.color = new Color(1, 1, 1, 1 - (timeElapsed / 2f));
            yield return new WaitForEndOfFrame();
        }

        MasterGameManager.instance.inputActive = true;
    }
}
