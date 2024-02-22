using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingBox : MonoBehaviour
{
    [SerializeField] Button SoundBtn;
    [SerializeField] Button ControlBtn;
    [SerializeField] Button BackBtn;

    [Header("Sound UI")]
    [SerializeField] GameObject SoundBackground;
    [SerializeField] Slider bgmVolume;
    [SerializeField] Toggle bgmMute;
    [SerializeField] TextMeshProUGUI bgmPercent;
    [SerializeField] Slider vfxVolume;
    [SerializeField] Toggle vfxMute;
    [SerializeField] TextMeshProUGUI vfxPercent;

    [Header("Control UI")]
    [SerializeField] GameObject ControlBackground;
    [SerializeField] Slider Sensitivity;
    [SerializeField] TextMeshProUGUI sensitivityPercent;
    [SerializeField] Slider distanceMax;
    [SerializeField] TextMeshProUGUI distanceMaxPercent;

    [Header("Ingmae Info")]
    [SerializeField] CameraMovement _camera;
    [SerializeField] MenuWindow _ingmaeMenu;

    private void Awake()
    {
        SoundBtn.onClick.AddListener(delegate { SettingSoundUI(); });
        ControlBtn.onClick.AddListener(delegate { SettingControlUI(); });
        BackBtn.onClick.AddListener(delegate { BackMenu(); });
        bgmVolume.onValueChanged.AddListener(delegate { SetSliderValue("bgmVolume", bgmVolume, bgmPercent); });
        bgmMute.onValueChanged.AddListener(delegate { SetToggleValue("bgmMute", bgmMute); });
        vfxVolume.onValueChanged.AddListener(delegate { SetSliderValue("sfxVolume", vfxVolume, vfxPercent); });
        vfxMute.onValueChanged.AddListener(delegate { SetToggleValue("sfxMute", vfxMute); });
        Sensitivity.onValueChanged.AddListener(delegate { SetSensitivityValue(); });
        distanceMax.onValueChanged.AddListener(delegate { SetdistanceMaxValue(); });
        SoundBackground.SetActive(true);
        ControlBackground.SetActive(false);
    }

    public void SetSettingBox()
    {
        this.gameObject.SetActive(true);
        bgmVolume.value = PlayerPrefs.GetFloat("bgmVolume");
        vfxVolume.value = PlayerPrefs.GetFloat("sfxVolume");
        bgmMute.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("bgmMute"));
        vfxMute.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("sfxMute"));
        bgmPercent.text = (float)Math.Round(bgmVolume.value * 100, 1) + "%";
        vfxPercent.text = (float)Math.Round(vfxVolume.value * 100, 1) + "%";
        Sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        distanceMax.value = PlayerPrefs.GetFloat("distanceMax");
        sensitivityPercent.text = Sensitivity.value + "%";
        distanceMaxPercent.text = distanceMax.value.ToString();
    }

    public void SettingSoundUI()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        SoundBackground.SetActive(true);
        ControlBackground.SetActive(false);

    }

    public void SettingControlUI()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        ControlBackground.SetActive(true);
        SoundBackground.SetActive(false);

    }

    public void BackMenu()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);

        this.gameObject.SetActive(false);
        if(_ingmaeMenu != null)
            _ingmaeMenu.gameObject.SetActive(true);
        else
            LobbyManager.Singleton.SetMenu();
    }



    #region [OnvalueChanged Methods]
    void SetSliderValue(string prefsText, Slider slider, TextMeshProUGUI percent)
    {
        PlayerPrefs.SetFloat(prefsText, slider.value);
        percent.text = (float)Math.Round(slider.value * 100, 1) + "%";
        AudioManager.Instance.SetMusicSlider();
        AudioManager.Instance.SetSfxSlider();
    }

    void SetToggleValue(string prefxText, Toggle toggle)
    {
        PlayerPrefs.SetInt(prefxText, Convert.ToInt32(toggle.isOn));
        AudioManager.Instance.SetMusicMute();
        AudioManager.Instance.SetSfxMute();
    }

    void SetSensitivityValue()
    {
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity.value);
        sensitivityPercent.text = Sensitivity.value + "%";
        if (_camera != null)
            _camera.SetSensitivity(Sensitivity.value);
    }

    void SetdistanceMaxValue()
    {
        PlayerPrefs.SetFloat("distanceMax", distanceMax.value);
        distanceMaxPercent.text = distanceMax.value.ToString();
        if (_camera != null)
            _camera.SetDistanceMax(distanceMax.value);
    }

    #endregion [OnvalueChanged Methods]
}
