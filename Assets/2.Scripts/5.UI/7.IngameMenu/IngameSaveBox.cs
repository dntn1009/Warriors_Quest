using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameSaveBox : MonoBehaviour
{
    [SerializeField] Button SaveBtn;
    [SerializeField] GameObject NotifyBox;
    [SerializeField] PlayerController player;
    private void Awake()
    {
        SaveBtn.onClick.AddListener(delegate { SetSaveBtn(); });
    }

    public void SetDataSave()
    {
        this.gameObject.SetActive(true);
        DataManager.Instance.PlayerDataSave(player._stat, Inventory.Singleton, IngameManager.Instance.currentMapState(), player.transform.position);
    }

    void SetSaveBtn()
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.BtnClick);
        NotifyBox.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
