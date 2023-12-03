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
        talkData.Add(1001, new string[] { "���� �ӽ����� �������� ū���̾�..", "���� ���� ���� �ٴҼ��� ����." });
        talkData.Add(1002, new string[] { "���谡 ��� �װ� �˰� �ֳ�?", "ȣ�� �ǳ��� ���̴� ���� ��ó�� �������� ��� �ִٳ�" });
        talkData.Add(1003, new string[] { "������ ��Ű�°� �� �ӹ��Դϴ�." });
        talkData.Add(1004, new string[] { "��� �߿� ��ȭ�� �����ؿ�. �����ּ���." });
        talkData.Add(1005, new string[] { "�� ���� �ٹ� �ð��̶��.. " });
        talkData.Add(1006, new string[] { "���� ���Ͱ� ���� �þ���־�. �����̾�." });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
