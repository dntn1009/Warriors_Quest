using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngamesaveQuitBox : MonoBehaviour
{
    [SerializeField] Button saveQuitBtn;
    [SerializeField] Button BackBtn;
    [SerializeField] PlayerController player;

    [SerializeField] GameObject NotifyBox;
    private void Awake()
    {
        saveQuitBtn.onClick.AddListener(delegate { SetsaveQuitBtn(); });
        BackBtn.onClick.AddListener(delegate { SetBackBtn(); });
    }

    void SetsaveQuitBtn()
    {
        DataManager.Instance.PlayerDataSave(player._stat, Inventory.Singleton, IngameManager.Instance.currentMapState(), player.transform.position);
        Application.Quit();
        //Data Save & Quit
    }

    void SetBackBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
