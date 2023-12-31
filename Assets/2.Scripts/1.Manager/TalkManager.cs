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

        //normal Talk
        talkData.Add(1001, new string[] { "요즘 머쉬룸이 많아져서 큰일이야..", "마을 밖을 돌아 다닐수가 없어." });
        talkData.Add(1002, new string[] { "모험가 양반 그거 알고 있나?", "호수 건너편에 보이는 절벽 근처에 슬라임이 살고 있다네" });
        talkData.Add(1003, new string[] { "마을을 지키는게 제 임무입니다." });
        talkData.Add(1004, new string[] { "경비 중에 대화는 위험해요. 비켜주세요." });
        talkData.Add(1005, new string[] { "곧 교대 근무 시간이라니.. " });
        talkData.Add(1006, new string[] { "요즘 몬스터가 점점 늘어나고있어. 걱정이야." });
        talkData.Add(2000, new string[] { "마을 밖에 무슨일이 생긴게 분명해..", "머쉬룸이 도시 안으로 들어오진 않겠지?" });
        talkData.Add(3000, new string[] { "밖에 머쉬룸들이 가득해..", "머쉬룸을 가까이서 보고싶어.." });
        talkData.Add(4000, new string[] { "한눈 팔수는 없습니다." });

        //Quest Talk
        talkData.Add(2000 + 1, new string[] { "안녕? 모험가로구나!", "시간이 있으면 마을 밖 머쉬룸 10마리를 잡아줄 수 있니?" }); // 머쉬룸 킬
        talkData.Add(3000 + 1, new string[] { "너가 겁많은 주민이 말하던 모험가로구나!", "마을 밖에 더 나갈 일이 있니?", "괜찮다면 머쉬룸의 갓을 10개 가져다 주지않을래?" }); // 머쉬룸 갓
        talkData.Add(4000 + 1, new string[] { "안녕하세요. 모험가님", "머쉬룸이 마을 근처까지 오게 된 이유를 알게 되었습니다.", "호수 너머에 있는 슬라임들이 머쉬룸을 내쫒은게 분명합니다.", "괜찮다면 슬라임을 10마리 처치하고 와주실 수 있겠습니까?" }); // 슬라임 킬

        //Quest Finish Talk
        talkData.Add(2000 + 2, new string[] { "어?! 머쉬룸을 다 잡고왔구나?!", "사소하지만 보상을 줄게. 다음에도 잘 부탁해!" }); // 머쉬룸 킬
        talkData.Add(3000 + 2, new string[] { "머쉬룸의 갓을 다 모으고 왔니?", "대단한 실력을 가졌구나. 약소하지마 내가 만든 물약이야. 다음에도 잘 부탁해!" }); // 머쉬룸 갓
        talkData.Add(4000 + 2, new string[] { "주민들의 이야기대로 대단한 실력을 가지셨군요.", "슬라임을 처치한 만큼 보상을 드리겠소. 다음에도 잘 부탁하오." }); // 슬라임 킬

    }

    public string GetTalk2(int id, int talkIndex)
    {
        if(!talkData.ContainsKey(id))
            if(!talkData.ContainsKey(id - id % 10))//퀘스트 맨 처음 대사마저 없을 때. 기본 대사를 가지고 온다.
                return GetTalk(id - id % 100, talkIndex);
            else //해당 퀘스트 진행 순서 대사가 없을 때. //퀘스트 맨 처음 대사를 가지고 온다.
                return GetTalk(id - id % 10, talkIndex);

        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
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
