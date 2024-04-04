using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralControls : MonoBehaviour
{
    public GameObject pauseMenu;
    KeyCode escape = KeyCode.Escape; //Using an assigned variable for the keycode so that it's still usable when keybinds are written in the game

    private void Update()
    {
        if (Input.GetKeyDown(escape)) 
        {
            TogglePause();
            pauseMenu.SetActive(!pauseMenu.activeSelf); //open the pause menu if it's closed, close it if it's open
        }
    }
    public static void TogglePause() //pause the game if it is active, resume it if it is paused
    {
        if (Time.timeScale == 1)
        {
            PauseGame();
        }

        else
        {
            ResumeGame();
        }
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
