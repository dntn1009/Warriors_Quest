using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Quest")]
    public int questIndex = -1;
    public QuestData _quest;
    [SerializeField] GameObject QuestNotifyObj;

    [Header("NPC Data")]
    public string NAME;
    public int ID;
    public bool isNPC;

    [Header("Shop Item")]
    public Item[] shopItem;

    private void Start()
    {
        if (questIndex != -1)
        {
            IngameManager.Instance.npcQuestSetting(this);
            if (!_quest.isActive)
                QuestNotifyObj.SetActive(true);
        }
    }

    public void NotifyActiveTrue()
    {
        if (_quest.isActive)
            QuestNotifyObj.GetComponent<TextMeshProUGUI>().text = "!";
        else
            QuestNotifyObj.GetComponent<TextMeshProUGUI>().text = "?";
    }

}
