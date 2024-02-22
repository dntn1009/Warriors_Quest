using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = DataManager.Instance.LoadTalkData();
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public bool QuestBtnSetActvie(int id, int talkIndex)
    {
        return (talkIndex == talkData[id].Length - 1);
    }
}
