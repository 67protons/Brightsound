using UnityEngine;
using System.Collections;

public class LightShot : MonoBehaviour
{
    BoxCollider2D hitbox;

    public LightHead head;
    public TrailRenderer wave1, wave2;

    public int weaponDamage = 1;

    public float velocity = 15f;
    public float frequency = 5f;
    public float magnitude = 0.5f;
    public float phaseShift = 0.5f;

    public float shotLength = .34f;
    public float lifespan = 1f;

    private float distanceCovered = 0f;

    void Awake()
    {
        hitbox = this.GetComponent<BoxCollider2D>();
    }

    public void Shoot(float angle)
    {
        AkSoundEngine.PostEvent("LightShot", this.gameObject);
        this.transform.Rotate(0f, 0f, angle);
        StartCoroutine(ShootCoroutine(shotLength));
        Destroy(this.gameObject, lifespan);
    }

    IEnumerator ShootCoroutine(float shotDuration)
    {
        while (distanceCovered <= shotLength && !head.hasCollided)
        {
            //Move center shot
            head.transform.localPosition += new Vector3(velocity * Time.deltaTime, 0f, 0f);

            //Begin calculation for wave1 and wave2
            Vector2 pos = head.transform.localPosition;
            hitbox.size = new Vector2(pos.x, hitbox.size.y);
            hitbox.offset = new Vector2(hitbox.size.x / 2, 0f);

            //distanceCovered = pos.x;
            //head.particles.main.startLifetime = distanceCovered;
            head.particles.startLifetime = distanceCovered * 0.05f;

            wave1.time = lifespan;
            wave2.time = lifespan;

            wave1.transform.localPosition = new Vector3(pos.x, Mathf.Sin(pos.x * frequency) * magnitude, 0f);
            wave2.transform.localPosition = new Vector3(pos.x, Mathf.Sin(pos.x * frequency + phaseShift * 3.14f) * magnitude, 0f);

            distanceCovered += velocity * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damage(weaponDamage);
        }
    }
}