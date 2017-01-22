using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health = 1;
    public int damage = 1;
    public SpriteRenderer sprite;

    public AudioClip[] damagedSounds;
    public AudioClip[] deathSounds;

    public void Damage(int damage)
    {
        this.health -= damage;
        StartCoroutine(BlinkRed());
        if (this.health <= 0)
        {
            Destroy(this.gameObject);
            MasterGameManager.instance.audioManager.randomSFX(deathSounds);
        }
        else
        {
            MasterGameManager.instance.audioManager.randomSFX(damagedSounds);
        }
    }

    IEnumerator BlinkRed()
    {
        //sprite.color = Color.white;
        float timeElapsed = 0f;

        while (timeElapsed <= 1f)
        {
            if (timeElapsed < .2f)
                sprite.color = Color.red;
            if (timeElapsed >= .2f && timeElapsed < .4f)
                sprite.color = Color.white;
            if (timeElapsed >= .4f && timeElapsed < .6f)
                sprite.color = Color.red;
            if (timeElapsed >= .6f)
                sprite.color = Color.white;

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
