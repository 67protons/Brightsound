using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterGameManager : MonoBehaviour {
    public static MasterGameManager instance = null;

    public SceneManagerWrapper sceneManager;
    public AudioManager audioManager;
    public bool inputActive = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
