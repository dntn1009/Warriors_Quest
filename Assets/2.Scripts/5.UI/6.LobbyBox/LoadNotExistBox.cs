using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadNotExistBox : MonoBehaviour
{
    [SerializeField] Button BackBtn;

    private void Awake()
    {
        BackBtn.onClick.AddListener(delegate { BackMenu(); });
    }

    public void BackMenu()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetMenu();
    }
}
