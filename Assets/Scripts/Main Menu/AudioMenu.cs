using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{

    void Awake()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
    }
    // Start is called before the first frame update
    void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        SfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);

        if (PlayerPrefs.HasKey("MasterVolume"))
        {

        }
        //Mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 0.75f));
    }

    public AudioMixer Mixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SfxSlider;

    public void SetMasterVolume(float volume)
    {
        Mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        Mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        Mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", volume);
    }

}
