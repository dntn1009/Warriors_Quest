using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;
using UnityEngine.SceneManagement;

public class NewStartBox : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] Button StartBtn;
    [SerializeField] Button BackBtn;

    private void Awake()
    {
        StartBtn.onClick.AddListener(delegate { StartIngame(); });
        BackBtn.onClick.AddListener(delegate { Backmenu(); });
    }

    public void StartIngame()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        DataManager.Instance._loadingmanager.SetLoading();
        DataManager.Instance.NewPlayerDataSave(nameInput.text, DataManager.Instance._StatData[0], MapType.Stage1, new Vector3(-380, 10, -490));
        DataManager.Instance.LoadScene(SceneType.IngameScene);
        //InputField str + Gold + Vecotror3 position + PlayerStatData(Json) => playData.json save
        // Scene 이동 및 Loading 구현
        // playerData IngamePlayerobj 넣기
    }

    public void Backmenu()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetMenu();
    }

}
