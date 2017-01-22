using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneManagerWrapper : MonoBehaviour
{

    Fader fader;
    Canvas canvas;
    [HideInInspector]
    public bool loadingScene = false;
    public string currentScene;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingScene = false;
        currentScene = scene.name;
        GameObject faderGO = GameObject.Find("Fader");
        GameObject canvasGO = GameObject.Find("Canvas");
        if (canvasGO != null)
        {
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.sortingOrder = 0;
        }

        if (faderGO != null)
        {
            fader = faderGO.GetComponent<Fader>();
            FadeToClear();
            StartCoroutine(DelaySceneStart(fader.fadeTime / 2));
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void LoadScene(string sceneName, bool hidePlayer)
    {
        //Prevents loading different scenes while fading.
        if (hidePlayer && canvas != null)
        {
            canvas.sortingOrder = 2;
        }

        if (loadingScene)
            return;
        loadingScene = true;

        if (fader != null)
        {
            Time.timeScale = 0;
            FadeToBlack();
            StartCoroutine(DelayedSceneLoad(sceneName, fader.fadeTime));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void FadeToBlack()
    {
        if (fader != null)
            fader.FadeToBlack();
    }

    public void FadeToClear()
    {
        if (fader != null)
            fader.FadeToClear();
    }

    IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator DelaySceneStart(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1;
    }
}
