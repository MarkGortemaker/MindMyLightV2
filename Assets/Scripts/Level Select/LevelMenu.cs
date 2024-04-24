using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons = new Button[7];
    private void Awake()
    {
        int currentLevel = PlayerPrefs.GetInt("levelProgress", 0); //levelProgress is increased after completed each level

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
}
