using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _uiSlider;

    private void Start()
    {
        _masterSlider.value = AudioManager.Instance.InitMasterVol;
        _musicSlider.value = AudioManager.Instance.InitMusicVol;
        _sfxSlider.value = AudioManager.Instance.InitSfxVol;
        _uiSlider.value = AudioManager.Instance.InitUiVol;
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }
    
    public void SetSfxVolume(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }

    public void SetUiVolume(float value)
    {
        AudioManager.Instance.SetUiVolume(value);
    }
}
