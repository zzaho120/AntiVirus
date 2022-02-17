using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void OnClickGameExit()
    {
        Application.Quit();
    }
}
