using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeOut : MonoBehaviour {

    public bool activated = false;
    public float fadeRate = .05f;
    public AudioSource trackSource;

    public void activateFadeOut() {
        activated = true;
        StartCoroutine(MasterGameManager.instance.audioManager.fadeOut(trackSource, fadeRate));
    }
}
