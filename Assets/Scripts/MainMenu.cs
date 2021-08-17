using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator PressAnyButtonPanel;
    public GameObject PressAnyButton;

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void AnyButton()
    {
        PressAnyButtonPanel.SetTrigger("hide");
        PressAnyButton.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
