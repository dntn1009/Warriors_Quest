using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    [Header("Talk UI")]
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _talkText;
    [SerializeField] PlayerController _player;

    [Header("QuestBtn UI")]
    [SerializeField] Button _questBtn;
    [SerializeField] TextMeshProUGUI _questTitle;

    bool _questTalkCheck = false;

    bool _isAction;
    public int talkIndex;

    public void TalkAction()
    {
        _questBtn.gameObject.SetActive(false);
        _isAction = true;
        NPCData _npcData = _player.npcObjSet().GetComponent<NPCData>();
        _nameText.text = _npcData.NAME;
        Talk(_npcData);
        this.gameObject.SetActive(_isAction);
    }

    void Talk2(int id, bool isNPC)
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

    void Talk(NPCData npcdata)
    {
        //대화내용 가져오는 부분
        int npcID = 0;

        if (!_questTalkCheck)
        {
            if(!npcdata.isNPC)
                npcID = npcdata.ID;
            else if (npcdata._quest.CompleteCheck())
                npcID = npcdata.ID + 2;
            else
                npcID = npcdata.ID;
        }
        else
            npcID = npcdata.ID + 1;

        string talkValue = IngameManager.Instance.GetTalk(npcID, talkIndex);
        if (talkValue == null)
        {
            if(!npcdata.isNPC)
            {
                IngameManager.Instance.ShopOpen(npcdata);
                _isAction = false;
                talkIndex = 0;
                return;
            }

            if (_questTalkCheck)
            {
                _questTalkCheck = false;
                IngameManager.Instance.RequestOpen(npcdata._quest.title, npcdata._quest.description, npcdata._quest.questGoal.questType);
            }

            if(npcdata._quest.CompleteCheck())
            {
                _player.QuestComplete();
                npcdata.NotifyActiveTrue();
            }

            _isAction = false;
            talkIndex = 0;
            return;
        }
        if (npcdata.isNPC)
            _talkText.text = talkValue;
        else
            _talkText.text = talkValue;

        //quest Btn 활성화 부분

        bool questbtnCheck = IngameManager.Instance.QuestBtnSetActvie(npcdata.ID, talkIndex);
        if (!npcdata._quest.isActive && questbtnCheck)
        {
            if (npcID % 1000 == 0 && npcdata.questIndex != -1)
            {
                _questBtn.gameObject.SetActive(true);
                if(_player._quest.isActive)
                {
                    _questBtn.interactable = false;
                }
                _questTitle.text = npcdata._quest.title;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;


            }
        }

        //대화 index 증가 및 Windw SetActive 부분
        _isAction = true;
        talkIndex++;
    }



    public void QuestClick()
    {
        _questBtn.gameObject.SetActive(false);
        _questTalkCheck = true;
        talkIndex = 0;
        _isAction = true;
        Talk(_player.npcObjSet().GetComponent<NPCData>());
    }
}
