using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public Fader fader;
    public Image startButton, quitButton;
    public AudioClip outInTheCold;
    public AudioSource cutsceneTrigger;

    void Start  ()
    {
        MasterGameManager.instance.inputActive = false;
    }

    public void ClickStart()
    {
        //MasterGameManager.instance.sceneManager.LoadScene("Level1", false);
        StartCoroutine(StartGame());
        //Placeholder
        AkSoundEngine.SetState("Level", "GGJ");
        MasterGameManager.instance.audioManager.PlayMusic(outInTheCold);
        cutsceneTrigger.Play();
    }

    IEnumerator StartGame()
    {
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        fader.FadeToClear();
        yield return new WaitForSeconds(fader.fadeTime / 2);

        MasterGameManager.instance.inputActive = true;
        this.gameObject.SetActive(false);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
