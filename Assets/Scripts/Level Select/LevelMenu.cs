using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons = new Button[7];
    private void Awake()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1); //current level will be manually increased and updated in playerprefs after each level completion

        buttons = gameObject.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            b.interactable = false;
        }

        for (int i = 0; i < currentLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
