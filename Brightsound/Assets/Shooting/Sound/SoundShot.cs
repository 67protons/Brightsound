using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShot : MonoBehaviour {

    public Collider2D soundCircle;
    private List<Collider2D> circleList = new List<Collider2D>();
    float minWidth, maxWidth, minHeight, maxHeight;
    public float velocity = 15f; 
    public float lifespan = 1f;
    public float shotLength = .35f;
    public float delayTime;
    private int numOfCircles = 5;
    [HideInInspector]
    public Vector2 aimDirection;
    public AudioClip shotSound;

    void Awake()
    {
        delayTime = shotLength / numOfCircles;
    }

    public void Shoot(float angle, Vector2 direction)
    {
        AudioManager.instance.PlaySFXClip(shotSound);
        this.transform.Rotate(0f, 0f, angle);
        this.aimDirection = direction;

        StartCoroutine(ShootCoroutine(shotLength));
        Destroy(this.gameObject, lifespan);
    }

    IEnumerator ShootCoroutine(float shotDuration)
    {
        

        float timeElapsed = 0f;
        int numShot = 1;
        while (timeElapsed <= lifespan)
        {
            if (timeElapsed <= shotDuration)
            {
                if (timeElapsed >= numShot * delayTime)
                {
                    Collider2D newSoundCircle = Instantiate(soundCircle, this.transform.position, this.transform.rotation);
                    newSoundCircle.transform.SetParent(this.transform);
                    circleList.Add(newSoundCircle);
                    numShot++;
                }
                //Move center shot
                foreach (Collider2D circle in circleList)
                {
                    circle.transform.localPosition += new Vector3(velocity * Time.deltaTime, 0f, 0f);
                    circle.transform.localScale += new Vector3(0f, 100f * Time.deltaTime, 0f);
                }
            }
            
            if (timeElapsed > .5f && timeElapsed <= lifespan)
            {
                foreach (Collider2D circle in circleList)
                {
                    SpriteRenderer circleSprite = circle.GetComponent<SpriteRenderer>();
                    float alphaPercentage = lifespan - (timeElapsed / lifespan);
                    circleSprite.color = new Color(circleSprite.color.r, circleSprite.color.g, circleSprite.color.b, alphaPercentage);
                }
            }
            

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
