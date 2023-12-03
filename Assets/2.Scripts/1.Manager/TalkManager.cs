using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        //1000 = Normal 2000 = Quest 3000 = Shop
        talkData.Add(1001, new string[] { "요즘 머쉬룸이 많아져서 큰일이야..", "마을 밖을 돌아 다닐수가 없어." });
        talkData.Add(1002, new string[] { "모험가 양반 그거 알고 있나?", "호수 건너편에 보이는 절벽 근처에 슬라임이 살고 있다네" });
        talkData.Add(1003, new string[] { "마을을 지키는게 제 임무입니다." });
        talkData.Add(1004, new string[] { "경비 중에 대화는 위험해요. 비켜주세요." });
        talkData.Add(1005, new string[] { "곧 교대 근무 시간이라니.. " });
        talkData.Add(1006, new string[] { "요즘 몬스터가 점점 늘어나고있어. 걱정이야." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
