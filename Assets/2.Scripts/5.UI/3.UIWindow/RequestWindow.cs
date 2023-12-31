using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DefineHelper;

public class RequestWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI type;

    [SerializeField] PlayerController _player;

    public void requestSetting(string title, string description, QuestType questType)
    {
        this.title.text = title;
        this.description.text = description;
        if (questType == QuestType.Kill)
            type.text = "처치하기";
        else if(questType == QuestType.Gathering)
            type.text = "수집하기";
    }

    public void acceptBtnClick()
    {
        _player._quest = _player.npcObjSet().GetComponent<NPCData>()._quest;
        _player._quest.isActive = true;
        _player.npcObjSet().GetComponent<NPCData>().NotifyActiveTrue();
        this.gameObject.SetActive(false);
        IngameManager.Instance.miniQuestOpen();
    }

    public void cancelBtnClick()
    {
        this.gameObject.SetActive(false);
    }
}
