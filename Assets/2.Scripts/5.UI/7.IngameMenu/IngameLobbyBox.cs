using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DefineHelper;

public class IngameLobbyBox : MonoBehaviour
{
    [SerializeField] Button LobbyBtn;
    [SerializeField] Button BackBtn;

    [SerializeField] GameObject NotifyBox;
    private void Awake()
    {
        LobbyBtn.onClick.AddListener(delegate { SetLobbyBtn(); });
        BackBtn.onClick.AddListener(delegate { SetBackBtn(); });
    }

    void SetLobbyBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        DataManager.Instance.LoadScene(SceneType.LobbyScene);
    }

    void SetBackBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
