using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    
    //play game from the main menu button, loads level 1, which should be build index 1
    public void playgame()
    {
        SceneManager.LoadScene("Build_0.2");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }





}
