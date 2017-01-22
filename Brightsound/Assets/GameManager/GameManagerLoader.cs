using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerLoader : MonoBehaviour {

    public GameObject gameManager;

    void Awake()
    {
        if (MasterGameManager.instance == null)
            Instantiate(gameManager);
    }
}
