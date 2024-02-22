using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadStartBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DateText;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI MapText;
    [SerializeField] Button LoadBtn;
    [SerializeField] Button BackBtn;

    private void Awake()
    {
        LoadBtn.onClick.AddListener(delegate { LoadIngame(); });
        BackBtn.onClick.AddListener(delegate { BackMenu(); });
    }

    void LoadIngame()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        DataManager.Instance._loadingmanager.SetLoading();
        DataManager.Instance.LoadScene(SceneType.IngameScene);
        //Json에서 받아온걸로 Ingame에 들어가서 Player에 적용
    }

    void BackMenu()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetMenu();
    }

    public void SetNotifyData(PlayerData playerdata)
    {
        this.gameObject.SetActive(true);
        DateText.text = "마지막 저장 : " + playerdata.date;
        NameText.text = playerdata.stat.NAME;
        MapText.text = playerdata.map.ToString();
    }
}
