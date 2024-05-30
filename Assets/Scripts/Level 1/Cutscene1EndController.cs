using AudioManager.Core;
using AudioManager.Locator;
using UnityEngine;

public class Cutscene1EndController : MonoBehaviour
{
    private IAudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = ServiceLocator.GetService();

        // Calling method in AudioManager
        am.Play("CutsceneMusic");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDisable()
    {
        am.Stop("CutsceneMusic");
    }
}
