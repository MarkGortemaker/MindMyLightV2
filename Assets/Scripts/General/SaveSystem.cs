using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    //public Button button;

    //private int _levelProgress;

    public int levelProgress
    {
        get => PlayerPrefs.GetInt("levelProgress", 0);
        private set
        {
            if (value >= PlayerPrefs.GetInt("levelProgress", 0))
            {
                PlayerPrefs.SetInt("levelProgress", value);
                Debug.Log($"Level {value + 1} unlocked");
            }
        }
    }

    [ContextMenu("Do Something")]
    public void SaveLevelProgress(int level) => levelProgress = level; // does the same as SaveSystem.levelProgress = level

    private int resetPressCount = 0;
    public void ResetLevelProgress()
    {
        PlayerPrefs.SetInt("levelProgress", 0);
        resetPressCount++;
        if (resetPressCount > 5)
        {
            levelProgress = 6;
            resetPressCount = 0;
            Debug.Log("Levels Unlocked!");
        }

    }
}
