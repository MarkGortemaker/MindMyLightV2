using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationMenu : EditorWindow
{
    static string[] localizations;
    
    /// <summary>
    /// Behaviour to be called upon this window being opened.
    /// </summary>
    public static void ShowWindow()
    {
        //Calls windows and sets sizes.
        LocalizationMenu window = GetWindow<LocalizationMenu>("Localization");
        window.minSize = new(300, 100);
        window.maxSize = new(1200, 500);

        localizations = AssetDatabase.GetSubFolders("Assets\\Resources\\DiaTextSystem"); //Gets all localization folders.
        for (int i = 0; i < localizations.Length; i++)
        {
            localizations[i] = localizations[i].Replace('/', '\\'); //Changes all forward slashed to backslashes.
            localizations[i] = localizations[i].Remove(0, 17); //Removes "Assets\\Resources\\" from path.
            Debug.Log(localizations[i]);
        }
    }

    private void OnGUI()
    {
        for (int i = 0; i < localizations.Length; i++)
        {
            if (GUILayout.Button(localizations[i], GUILook.labelLayout))
            {
                SetLocale(localizations[i]);
            }
        }
    }

    void SetLocale(string locale)
    {
        PlayerPrefs.SetString("JSONDir", locale);
        Debug.Log(PlayerPrefs.GetString("JSONDir"));
        JSONView.SortChapters();
    }
}
