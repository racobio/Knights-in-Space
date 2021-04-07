using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseUI;

    // Adds pause button keybind to escape
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
                Pause();
        }
    }
    //resumes the game and closes pause menu
    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }
    //stops the game and opens pause menu
    private void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
    //loads other menus
    public void LoadMenu()
    {
        //Main Menu is scene 0, sets timescale back to normal to ensure proper functioning
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    //exits game
    public void QuitGame()
    {
        Application.Quit();
    }
}
