using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == ("FadeInTrigger"))
        {
            //grab the script to check if audio needs to be faded in
            AudioFadeIn fadeScript = other.gameObject.GetComponent<AudioFadeIn>();
            if (!fadeScript.activated)
                fadeScript.activateFadeIn();
            
        }

        if (other.gameObject.name == ("FadeOutTrigger")) {
            //grab the script to check if audio needs to be faded in
            AudioFadeOut fadeScript = other.gameObject.GetComponent<AudioFadeOut>();
            if (!fadeScript.activated)
                fadeScript.activateFadeOut();

        }
    }
}