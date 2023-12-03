using TMPro;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _talkText;
    [SerializeField] PlayerController _player;
    bool _isAction;
    public int talkIndex;

    public void TalkAction()
    {
            _isAction = true;
            NPCData _npcData = _player.npcObjSet().GetComponent<NPCData>();
            _nameText.text = _npcData.NAME;
            Talk(_npcData.ID, _npcData.isNPC);
            this.gameObject.SetActive(_isAction);
    }

    void Talk(int id, bool isNPC)
    {
        string talkValue = IngameManager.Instance.GetTalk(id, talkIndex);

        if (talkValue == null)
        {
            _isAction = false;
            talkIndex = 0;
            return;
        }

        if (isNPC)
            _talkText.text = talkValue;
        else
            _talkText.text = talkValue;
        _isAction = true;
        talkIndex++;
    }
}
