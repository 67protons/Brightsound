using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    Image black;
    public float fadeTime = 1.5f;

    void Awake()
    {
        black = this.GetComponent<Image>();
    }

    public void FadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(BlackCoroutine());
    }

    public void FadeToClear()
    {
        StopAllCoroutines();
        StartCoroutine(ClearCoroutine());
    }

    IEnumerator BlackCoroutine()
    {
        black.color = Color.clear;
        float timeElapsed = 0;

        while (black.color.a <= 0.99f)
        {
            timeElapsed += Time.unscaledDeltaTime;
            black.color = new Color(0, 0, 0, timeElapsed / fadeTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ClearCoroutine()
    {
        black.color = Color.black;
        float timeElapsed = 0;

        while (black.color.a >= 0.01f)
        {
            timeElapsed += Time.unscaledDeltaTime;
            black.color = new Color(0, 0, 0, 1 - (timeElapsed / fadeTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
