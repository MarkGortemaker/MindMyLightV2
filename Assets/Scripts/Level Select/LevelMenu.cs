using AudioManager.Core;
using AudioManager.Locator;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private IAudioManager am;
    public Button[] buttons = new Button[7];
    private void Awake()
    {
        am = ServiceLocator.GetService();
        int currentLevel = PlayerPrefs.GetInt("levelProgress", 0); //levelProgress is increased after completing each level

        buttons = gameObject.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            b.interactable = false;
        }

        for (int i = 0; i < currentLevel + 1; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void OnDisable()
    {
        am.Stop("MainMenuMusic");
    }
}
