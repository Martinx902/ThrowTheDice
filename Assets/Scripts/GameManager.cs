using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject controlsPanel;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Controls()
    {
        if(controlsPanel.activeSelf)
        {
            controlsPanel.SetActive(false);
        }
        else
        {
            controlsPanel.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
