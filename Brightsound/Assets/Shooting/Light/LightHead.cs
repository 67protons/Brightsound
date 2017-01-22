using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHead : MonoBehaviour {

    public ParticleSystem particles;
    [HideInInspector]
    public bool hasCollided = false;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Lightwave"))
        {
            hasCollided = true;
        }
    }
}
