using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GraphicsMenu : MonoBehaviour
{
    public UnityEngine.UI.Toggle fullscreenToggle, vSyncToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    List<string> resolutionTexts = new();
    List<int[]> resolutions = new();

    int resWidth, resHeight;

    void Start()
    {
        resWidth = Display.main.systemWidth; //get native width and height values
        resHeight = Display.main.systemHeight;

        fullscreenToggle.isOn = Screen.fullScreen; 
        vSyncToggle.isOn = QualitySettings.vSyncCount != 0;
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        foreach (Resolution r in Screen.resolutions)
        {
            if (!resolutionTexts.Contains(r.width + "x" + r.height) && (double)r.width / r.height == (double)16 / 9)
            {
                resolutionTexts.Add(r.width + "x" + r.height);
                resolutions.Add(new int[2] { r.width, r.height });
            }
        }
        
        resolutionTexts.Insert(0, resWidth + "x" + resHeight + "(Native)");
        resolutions.Insert(0, new int[2] { resWidth, resHeight });

        resolutionDropdown.AddOptions(resolutionTexts);
    }

    public void GetResolution()
    {
        resWidth = resolutions.ToArray()[resolutionDropdown.value][0];
        resHeight = resolutions.ToArray()[resolutionDropdown.value][1];
    }

    public void ApplyChanges()
    {
        Screen.SetResolution(resWidth, resHeight, fullscreenToggle.isOn);
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }
}
