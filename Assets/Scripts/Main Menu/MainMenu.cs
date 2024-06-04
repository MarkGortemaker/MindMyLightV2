using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void SetLocalization(int localization)
    {
        switch (localization)
        {
            case 0: PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\ENG");
                break;
            case 1: PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\NL");
                break;
            case 2: PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\UKR");
                break;
            default:
                break;
        }
        Debug.Log(PlayerPrefs.GetString("JSONDir"));
    }

}
