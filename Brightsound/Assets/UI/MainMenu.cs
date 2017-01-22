using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public void ClickStart()
    {
        MasterGameManager.instance.sceneManager.LoadScene("Level1", false);
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
