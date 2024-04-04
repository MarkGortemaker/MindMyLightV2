using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static void LoadLevel(string level)
    {
        SceneManager.LoadSceneAsync(level);
        GeneralControls.canPause = true;
    }

    public static void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        GeneralControls.canPause = true;
    }
}
