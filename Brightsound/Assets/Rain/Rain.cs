using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour {

    Player player;
    float offsetY;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        offsetY = this.transform.position.y;
    }

    void LateUpdate()
    {
        this.transform.position = new Vector3(player.transform.position.x, offsetY, 0f);
    }
}
