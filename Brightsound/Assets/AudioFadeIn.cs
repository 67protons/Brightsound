using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour {
    public bool activated = false;
    public float fadeRate = .05f;
    public AudioSource trackSource;

    private void Start()
    {
        //Temporary fix, need to find a way to destroy these later
        DontDestroyOnLoad(this.gameObject);
    }

    public void activateFadeIn() 
    {
        activated = true;
        StartCoroutine(MasterGameManager.instance.audioManager.fadeIn(trackSource, fadeRate));
    }
}
