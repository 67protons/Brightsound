using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int health = 1;
    public int damage = 1;
    public SpriteRenderer sprite;

    //This is a temp workaround for now
    //Better way is probably for each enemy to be their own subclass of the enemy class
    public string EnemyName;

    public void Damage(int damage)
    {
        this.health -= damage;
        StartCoroutine(BlinkRed());
        if (this.health <= 0)
        {
            Destroy(this.gameObject);
            if (EnemyName == "Cello")
                AkSoundEngine.PostEvent("CelloDeath", this.gameObject);
            if (EnemyName == "Robot")
                AkSoundEngine.PostEvent("RobotDeath", this.gameObject);
        }
        else
        {
            if (EnemyName == "Cello")
                AkSoundEngine.PostEvent("CelloDamaged", this.gameObject);
            if (EnemyName == "Robot")
                AkSoundEngine.PostEvent("RobotDamaged", this.gameObject);
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
