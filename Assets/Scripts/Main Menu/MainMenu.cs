using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        if (!PlayerPrefs.HasKey("JSONDir"))
        {
            PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\ENG");
        }
        //set default localization to English
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetLocalization(int localization)
    {
        switch (localization)
        {
            case 0:
                PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\ENG");
                break;
            case 1:
                PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\NL");
                break;
            case 2:
                PlayerPrefs.SetString("JSONDir", "DiaTextSystem\\UKR");
                break;
            default:
                Debug.LogWarning("Unknown localization.");
                break;
        }
        Debug.Log(PlayerPrefs.GetString("JSONDir"));
    }
}
