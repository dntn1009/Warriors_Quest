using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IngameQuitBox : MonoBehaviour
{
    [SerializeField] Button QuitBtn;
    [SerializeField] Button BackBtn;

    [SerializeField] GameObject NotifyBox;
    private void Awake()
    {
        QuitBtn.onClick.AddListener(delegate { SetQuitBtn(); });
        BackBtn.onClick.AddListener(delegate { SetBackBtn(); });
    }

    void SetQuitBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        Application.Quit();
    }

    void SetBackBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
