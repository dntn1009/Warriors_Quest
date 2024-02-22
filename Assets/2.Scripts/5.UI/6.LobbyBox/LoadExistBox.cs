using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadExistBox : MonoBehaviour
{
    [SerializeField] Button okBtn;
    [SerializeField] Button cancelBtn;

    private void Start()
    {
        okBtn.onClick.AddListener(delegate { NewPlayerName();  });
        cancelBtn.onClick.AddListener(delegate { GoMenu(); });
    }

    public void NewPlayerName()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetNewStartBox();
    }

    public void GoMenu()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetMenu();
    }
}
