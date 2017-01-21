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

    void Awake()
    {
        delayTime = shotLength / numOfCircles;
    }

    public void Shoot(float angle, Vector2 direction)
    {
        this.transform.Rotate(0f, 0f, angle);
        this.aimDirection = direction;

        StartCoroutine(ShootCoroutine(shotLength));
        Destroy(this.gameObject, lifespan);
    }

    IEnumerator ShootCoroutine(float shotDuration)
    {
        

        float timeElapsed = 0f;
        int numShot = 1;
        while (timeElapsed <= shotDuration)
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
                circle.transform.localScale += new Vector3(0f, 1.25f, 0f);
            }

            

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
