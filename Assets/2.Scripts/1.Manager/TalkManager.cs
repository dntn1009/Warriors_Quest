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
        talkData.Add(1001, new string[] { "���� �ӽ����� �������� ū���̾�..", "���� ���� ���� �ٴҼ��� ����." });
        talkData.Add(1002, new string[] { "���谡 ��� �װ� �˰� �ֳ�?", "ȣ�� �ǳ��� ���̴� ���� ��ó�� �������� ��� �ִٳ�" });
        talkData.Add(1003, new string[] { "������ ��Ű�°� �� �ӹ��Դϴ�." });
        talkData.Add(1004, new string[] { "��� �߿� ��ȭ�� �����ؿ�. �����ּ���." });
        talkData.Add(1005, new string[] { "�� ���� �ٹ� �ð��̶��.. " });
        talkData.Add(1006, new string[] { "���� ���Ͱ� ���� �þ���־�. �����̾�." });
        talkData.Add(2000, new string[] { "���� �ۿ� �������� ����� �и���..", "�ӽ����� ���� ������ ������ �ʰ���?" });
        talkData.Add(3000, new string[] { "�ۿ� �ӽ������ ������..", "�ӽ����� �����̼� ����;�.." });
        talkData.Add(4000, new string[] { "�Ѵ� �ȼ��� �����ϴ�." });

        //Quest Talk
        talkData.Add(2000 + 1, new string[] { "�ȳ�? ���谡�α���!", "�ð��� ������ ���� �� �ӽ��� 10������ ����� �� �ִ�?" }); // �ӽ��� ų
        talkData.Add(3000 + 1, new string[] { "�ʰ� �̸��� �ֹ��� ���ϴ� ���谡�α���!", "���� �ۿ� �� ���� ���� �ִ�?", "�����ٸ� �ӽ����� ���� 10�� ������ ����������?" }); // �ӽ��� ��
        talkData.Add(4000 + 1, new string[] { "�ȳ��ϼ���. ���谡��", "�ӽ����� ���� ��ó���� ���� �� ������ �˰� �Ǿ����ϴ�.", "ȣ�� �ʸӿ� �ִ� �����ӵ��� �ӽ����� ���i���� �и��մϴ�.", "�����ٸ� �������� 10���� óġ�ϰ� ���ֽ� �� �ְڽ��ϱ�?" }); // ������ ų

        //Quest Finish Talk
        talkData.Add(2000 + 2, new string[] { "��?! �ӽ����� �� ���Ա���?!", "��������� ������ �ٰ�. �������� �� ��Ź��!" }); // �ӽ��� ų
        talkData.Add(3000 + 2, new string[] { "�ӽ����� ���� �� ������ �Դ�?", "����� �Ƿ��� ��������. ��������� ���� ���� �����̾�. �������� �� ��Ź��!" }); // �ӽ��� ��
        talkData.Add(4000 + 2, new string[] { "�ֹε��� �̾߱��� ����� �Ƿ��� �����̱���.", "�������� óġ�� ��ŭ ������ �帮�ڼ�. �������� �� ��Ź�Ͽ�." }); // ������ ų

    }

    public string GetTalk2(int id, int talkIndex)
    {
        if(!talkData.ContainsKey(id))
            if(!talkData.ContainsKey(id - id % 10))//����Ʈ �� ó�� ��縶�� ���� ��. �⺻ ��縦 ������ �´�.
                return GetTalk(id - id % 100, talkIndex);
            else //�ش� ����Ʈ ���� ���� ��簡 ���� ��. //����Ʈ �� ó�� ��縦 ������ �´�.
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
