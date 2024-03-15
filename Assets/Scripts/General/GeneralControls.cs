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
            PauseResumeGame(pauseMenu.activeSelf); 
            pauseMenu.SetActive(!pauseMenu.activeSelf); //open the pause menu if it's closed, close it if it's open
        }
    }
    public void PauseResumeGame(bool IsPaused) //pause the game if it is active, resume it if it is paused
    {
        if (IsPaused == false)
        {
            Time.timeScale = 0;
        }

        else
        {
            Time.timeScale = 1;
        }
    }
}
