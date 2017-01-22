using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour {
    public bool activated = false;
    public float fadeRate = .05f;
    public AudioSource trackSource;

    public void activateFadeIn() 
    {
        activated = true;
        StartCoroutine(MasterGameManager.instance.audioManager.fadeIn(trackSource, fadeRate));
    }
}
