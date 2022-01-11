using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        References.audioManager.Play("menu-click");
        SceneManager.LoadScene("2PlayerGame");
    }
    public void MouseOver()
    {
        References.audioManager.Play("menu-hover");
    }
}
