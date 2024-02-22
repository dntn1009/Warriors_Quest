using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button SaveBtn;
    [SerializeField] Button saveQuitBtn;
    [SerializeField] Button SettingBtn;
    [SerializeField] Button LobbyBtn;
    [SerializeField] Button QuitBtn;

    [Header("UIBox")]
    [SerializeField] GameObject NotifyBox;
    [SerializeField] IngameSaveBox SaveBox;
    [SerializeField] IngamesaveQuitBox saveQuitBox;
    [SerializeField] IngameLobbyBox LobbyBox;
    [SerializeField] IngameQuitBox QuitBox;
    [SerializeField] SettingBox SettingBox;

    private void Awake()
    {
        SaveBtn.onClick.AddListener(delegate { SetSaveBtn(); });
        saveQuitBtn.onClick.AddListener(delegate { SetsaveQuitBtn(); });
        SettingBtn.onClick.AddListener(delegate { SetSettingBtn(); });
        LobbyBtn.onClick.AddListener(delegate { SetLobbyBtn(); });
        QuitBtn.onClick.AddListener(delegate { SetQuitBtn(); });
    }

    #region [Btn Methods]
    void SetSaveBtn()
    {
        //DataSave
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(true);
        SaveBox.SetDataSave();
    }

    void SetsaveQuitBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(true);
        saveQuitBox.gameObject.SetActive(true);
    }

    void SetLobbyBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(true);
        LobbyBox.gameObject.SetActive(true);
    }

    void SetSettingBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        SettingBox.SetSettingBox();
    }

    void SetQuitBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(true);
        QuitBox.gameObject.SetActive(true);
    }

    #endregion [Btn Methods]
}
