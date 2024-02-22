using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Singleton;

    [Header("Button")]
    [SerializeField] Button NewBtn;
    [SerializeField] Button LoadBtn;
    [SerializeField] Button SettingBtn;
    [SerializeField] Button ExitBtn;

    [Header("UIBOX")]
    [SerializeField] GameObject Menu; // 버튼 메뉴
    [SerializeField] DataLoadBox _DataLoadBox;
    [SerializeField] LoadExistBox _LoadExistBox; // 시작하기 클릭 시 LoadData 있는 경우
    [SerializeField] NewStartBox _NewStartBox; // 시작하기
    [SerializeField] LoadNotExistBox _LoadNotExistBox; // 불러오기 클릭 시 LoadData 없는 경우
    [SerializeField] LoadStartBox _LoadStartBox; // 불러오기
    [SerializeField] SettingBox _SettingBox; // 환경 설정

    private void Awake()
    {
        Singleton = this;
        Menu.SetActive(false);
        _LoadExistBox.gameObject.SetActive(false);
        _NewStartBox.gameObject.SetActive(false);
        _LoadNotExistBox.gameObject.SetActive(false);
        _LoadStartBox.gameObject.SetActive(false);
        _DataLoadBox.gameObject.SetActive(true);
        NewBtn.onClick.AddListener(delegate { SetNewBtn(); });
        LoadBtn.onClick.AddListener(delegate { SetLoadBtn(); });
        SettingBtn.onClick.AddListener(delegate { SetSettingBtn(); });
        ExitBtn.onClick.AddListener(delegate { SetExitBtn(); });
    }

    #region [Button Methods]
    void SetNewBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        Menu.SetActive(false);
        Debug.Log(DataManager.Instance.path);
        if (DataManager.Instance.PlayerDataCheck())
            SetLoadExistBox();
        else
            SetNewStartBox();
    }

    void SetLoadBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        Menu.SetActive(false);
        if (DataManager.Instance.PlayerDataCheck())
            SetLoadStartBox();
        else
            SetLoadNotExistBox();
    }

    void SetSettingBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        Menu.SetActive(false);
        SetSettingBox();
        //if
    }

    void SetExitBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        Application.Quit();
    }

    #endregion [Button Methods]

    #region [UIBOX Methods]
    public void SetMenu()
    {
        Menu.SetActive(true);
    }
    public void SetLoadExistBox()
    {
        _LoadExistBox.gameObject.SetActive(true);
    }
    public void SetNewStartBox()
    {
        _NewStartBox.gameObject.SetActive(true);
    }
    public void SetLoadNotExistBox()
    {
        _LoadNotExistBox.gameObject.SetActive(true);
    }
    public void SetLoadStartBox()
    {
        _LoadStartBox.SetNotifyData(DataManager.Instance.PlayerDataLoad());
    }
    public void SetSettingBox()
    {
        _SettingBox.SetSettingBox();
    }
    #endregion [UIBOX Methods]

    #region [Data Load Methods]
    public void DataLoadSuccess()
    {
        _DataLoadBox.gameObject.SetActive(false);
        Menu.SetActive(true);
    }

    public void DataLoadFailure()
    {
        _DataLoadBox.FailureLoad();
    }
    public void DataLoadBack()
    {
        _DataLoadBox.BackLoad();
    }
    #endregion [Data Load Methods]

}
