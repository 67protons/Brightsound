using UnityEngine;
using System.Collections;

public class LightShot : MonoBehaviour
{
    public Transform centerShot;
    public TrailRenderer wave1, wave2;

    public float velocity = 15f;
    public float frequency = 5f;
    public float magnitude = 0.5f;

    public float shotLength = .34f;
    public float lifespan = 1f;

    public AudioClip shotSound;

    public void Shoot(float angle)
    {
        AudioManager.instance.PlaySFXClip(shotSound);
        this.transform.Rotate(0f, 0f, angle);
        StartCoroutine(ShootCoroutine(shotLength));
        Destroy(this.gameObject, lifespan);
    }

    IEnumerator ShootCoroutine(float shotDuration)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= shotDuration)
        {
            //Move center shot
            centerShot.transform.localPosition += new Vector3(velocity * Time.deltaTime, 0f, 0f);

            //Begin calculation for wave1 and wave2
            Vector2 pos = centerShot.transform.localPosition;
            //Debug.Log(pos);
            wave1.transform.localPosition = new Vector3(pos.x, Mathf.Sin(pos.x * frequency) * magnitude, 0f);
            wave2.transform.localPosition = new Vector3(pos.x, -Mathf.Sin(pos.x * frequency) * magnitude, 0f);

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}