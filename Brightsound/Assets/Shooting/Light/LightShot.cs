using UnityEngine;
using System.Collections;

public class LightShot : MonoBehaviour
{
    public TrailRenderer trailRenderer;

    float velocity = 10f;
    float frequency = 20.0f;
    float magnitude = 0.5f;

    float shotDuration = .5f;

    float angle;

    public void Shoot(float angle)
    {
        Debug.Log(angle);
        this.angle = angle * Mathf.Deg2Rad;
        StartCoroutine(ShootCoroutine(shotDuration));
    }

    IEnumerator ShootCoroutine(float shotDuration)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= shotDuration)
        {
            Vector3 pos = trailRenderer.transform.position;
            float xPrime = pos.x * Mathf.Cos(angle) + pos.y * Mathf.Sin(angle);
            float yPrime = -pos.x * Mathf.Sin(angle) + pos.y * Mathf.Cos(angle);

            float x = xPrime + velocity * Time.deltaTime;
            float y = yPrime + Mathf.Sin(pos.x * frequency) * magnitude;

            xPrime = (x * Mathf.Cos(angle)) - (y * Mathf.Sin(angle));
            yPrime = (x * Mathf.Sin(angle)) + (y * Mathf.Cos(angle));

            trailRenderer.transform.position = new Vector3(xPrime, yPrime, 0f);
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}