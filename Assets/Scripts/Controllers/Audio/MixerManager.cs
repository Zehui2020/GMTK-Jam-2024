using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    // Audio
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    // Player prefs
    [SerializeField] private PlayerSettings playerSettings;

    private void Start()
    {
        SetSliders();
        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value > 0 ? Mathf.Log10(masterSlider.value) * 20 : -80f;
        SetVolume("Master", volume);
        playerSettings.masterVolume = masterSlider.value;
    }

    public void SetBGMVolume()
    {
        float volume = bgmSlider.value > 0 ? Mathf.Log10(bgmSlider.value) * 20 + 5 : -80f;
        SetVolume("BGM", volume);
        playerSettings.bgmVolume = bgmSlider.value;
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value > 0 ? Mathf.Log10(sfxSlider.value) * 20 : -80f;
        SetVolume("SFX", volume);
        playerSettings.sfxVolume = sfxSlider.value;
    }

    public void ResetVolume()
    {
        playerSettings.ResetVolume();

        masterSlider.value = playerSettings.masterVolume;
        bgmSlider.value = playerSettings.bgmVolume;
        sfxSlider.value = playerSettings.sfxVolume;

        SetMasterVolume();
        SetBGMVolume();
        SetSFXVolume();
    }

    public void SetSliders()
    {
        masterSlider.value = playerSettings.masterVolume;
        bgmSlider.value = playerSettings.bgmVolume;
        sfxSlider.value = playerSettings.sfxVolume;
    }

    private void SetVolume(string name, float volume)
    {
        audioMixer.SetFloat(name, volume);
    }

    private void OnApplicationQuit()
    {
        ResetVolume();
    }
}
