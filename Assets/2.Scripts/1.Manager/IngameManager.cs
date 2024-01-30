using DefineHelper;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameManager : SingletonMonobehaviour<IngameManager>
{
    [Header("Manager")]
    [SerializeField] TalkManager talkManager;
    [SerializeField] QuestManager questManager;
 
    [Header("Demage")]
    [SerializeField] Transform DamageManager;
    [SerializeField] GameObject DamageObject;

    [Header("WindowUI")]
    [SerializeField] Inventory _Inventory;
    [SerializeField] GameObject _MapWindow;
    [SerializeField] GameObject _StatWindow;
    [SerializeField] GameObject _SkillWindow;
    [SerializeField] GameObject _QuestWindow;
    [SerializeField] GameObject _MenuWindow;
    [SerializeField] GameObject _TalkWindow;
    [SerializeField] GameObject _RequestWindow;
    [SerializeField] MiniquestWindow _miniQuestWindow;
    [SerializeField] ShopWindow _ShopWindow;

    [Header("GetInfoText")]
    [SerializeField] GetInfo _GetInfo;

    [Header("Skill & Hot Bar")]
    [SerializeField] Image Skillbar_Q;
    [SerializeField] TextMeshProUGUI SkillText_Q;
    [SerializeField] Image Skillbar_E;
    [SerializeField] TextMeshProUGUI SkillText_E;
    [SerializeField] Image Skillbar_R;
    [SerializeField] TextMeshProUGUI SkillText_R;

    //���� ����
    MapType _currentMap;
    int _spawnMAX; // �ִ� ��ȯ�� �� �ִ� ���͵� ��
    public int _spawnNum;
    Coroutine _runningCoroutine;

    RaycastHit hit;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
       InitMap(MapType.Stage1); // �ӽ÷� Stage1�� ����
        //�÷��̾ �ҷ��ö� ��ġ�� ���� ���������� �ҷ��� ����.
    }

    private void Update()
    {
        MapState();
        if (Input.GetKeyDown(KeyCode.I))//�κ��丮 ��ư
        {
            InventoryOpen();
        }
        if (Input.GetKeyDown(KeyCode.P))//���� ��ư
        {
            StatOpen();
        }
        if (Input.GetKeyDown(KeyCode.K))//��ų ��ư
        {
            SkillOpen();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            QuestOpen();
        }
        if (Input.GetKeyDown(KeyCode.U))//�޴� ��ư
        {
            MenuOpen();
        }
        if (Input.GetKeyDown(KeyCode.M)) // ���� ��ư
        {
            MapOpen();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }


        //Potion
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Singleton.keyUsePotion(0);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Singleton.keyUsePotion(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Inventory.Singleton.keyUsePotion(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Inventory.Singleton.keyUsePotion(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Inventory.Singleton.keyUsePotion(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Inventory.Singleton.keyUsePotion(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Inventory.Singleton.keyUsePotion(6);
        }
    }

    #region [Map & Spawn Methods]
    public void MapState()
    {
        if (_runningCoroutine == null)
        {
            if (_spawnNum < _spawnMAX)
            {
                _spawnNum++;
                _runningCoroutine = StartCoroutine(MonsterReSpwan());
            }
        }
    }

    public void InitMap(MapType _type)
    {
        _currentMap = _type;
        SceneManager.LoadScene(_currentMap.ToString(), LoadSceneMode.Additive);
        Invoke("SetActiveScene", 0.1f);
    }

    public void ChangeMapFromMapType(MapType _type)
    {
        //���� ����ǵ��� ���� addtive.
        SceneManager.UnloadSceneAsync(_currentMap.ToString());
        _currentMap = _type;
        SceneManager.LoadScene(_currentMap.ToString(), LoadSceneMode.Additive);
        Invoke("SetActiveScene", 0.1f);
    }

    public void SetActiveScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentMap.ToString()));
        _spawnMAX = _spawnNum = MonsterManager.Instance._genPosition.Length;

    }

    #endregion [Map & Spawn Methods]


    #region [Damage UI Methods]

    public void CreateDamage(Vector3 position, string text, Color color)
    {
        var obj = Instantiate(DamageObject, position, Quaternion.identity);
        obj.transform.SetParent(DamageManager);
        var temp = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;
        Destroy(obj, 1f);
    }

    public void EffectParent(GameObject obj)
    {
        obj.transform.SetParent(DamageManager);
    }

    #endregion [Damage UI Methods]

    #region [Window UI Methods]
    public void ShopOpen(NPCData npc)
    {
        if (!_ShopWindow.hideCheck)
        {
            _ShopWindow.hideCheck = true;
            _ShopWindow.transform.localPosition = _ShopWindow.truePos;
            _ShopWindow.BuyActiveTrue();
            _ShopWindow.buySlotSetting(npc);
            _ShopWindow.sellSlotSetting(_Inventory);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void ShopClose()
    {
        if (_ShopWindow.hideCheck)
        {
            _ShopWindow.hideCheck = false;
            _ShopWindow.buySlotNull();
            _ShopWindow.sellSlotNull();
            _ShopWindow.truePos = _ShopWindow.gameObject.GetComponent<RectTransform>().localPosition;
            _ShopWindow.transform.localPosition = _ShopWindow.falsePos;
        }
    }

    public bool ShopCheck()
    {
        return _ShopWindow.hideCheck;
    }
    public void InventoryOpen()
    {
        if (!_Inventory.hideCheck)
        {
            if (_ShopWindow.hideCheck)
                return;

            _Inventory.hideCheck = true;
            _Inventory.transform.localPosition = _Inventory.truePos;
            _Inventory.SetGoldInfo();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            _Inventory.hideCheck = false;
            _Inventory.truePos = _Inventory.gameObject.GetComponent<RectTransform>().localPosition;
            _Inventory.transform.localPosition = _Inventory.falsePos;
            _ShopWindow.buySlotNull();
        }
    }
    public void MapOpen()
    {
        if (_MapWindow.activeSelf)
        {
            _MapWindow.SetActive(false);
        }
        else
        {
            _MapWindow.SetActive(true);
            _MapWindow.GetComponent<MapWindow>().SetPosXYInfo();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    public void StatOpen()
    {
        if (_StatWindow.activeSelf)
        {
            _StatWindow.SetActive(false);
        }
        else
        {
            _StatWindow.SetActive(true);
            _StatWindow.GetComponent<StatWindow>().SetStatInfo();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    public void SkillOpen()
    {
        if (_SkillWindow.activeSelf)
        {
            _SkillWindow.SetActive(false);
        }
        else
        {
            _SkillWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void QuestOpen()
    {
        if (_QuestWindow.activeSelf)
        {
            _QuestWindow.SetActive(false);
        }
        else
        {
            _QuestWindow.SetActive(true);
            _QuestWindow.GetComponent<QuestWindow>().QuestSetting();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    public void MenuOpen()
    {
        if (_MenuWindow.activeSelf)
        {
            _MenuWindow.SetActive(false);
        }
        else
        {
            _MenuWindow.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    public void miniQuestOpen()
    {
        _miniQuestWindow.miniQuestSetting();
    }

    public void miniQuestComplete()
    {
        _miniQuestWindow.completeSetting();
    }

    public void QuestRefresh()
    {
        _QuestWindow.GetComponent<QuestWindow>().amountSetting();
        _miniQuestWindow.currentAmountChange();
    }
    #endregion [Window UI Methods]

    #region [GetInfo Methods]
    public void SetGetInfoText(string text)
    {
        _GetInfo.setQueText(text);
    }

    #endregion [GetInfo Methods]

    #region [Manager Methods]

    //Talk Manager
    public void TalkOpen()
    {
        if (!_TalkWindow.activeSelf)
            _TalkWindow.SetActive(true);

        _TalkWindow.GetComponent<NPCTalk>().TalkAction();
    }

    public void equipMentStatInfo()
    {
        if (_StatWindow.activeSelf)
            _StatWindow.GetComponent<StatWindow>().SetStatInfo();
    }

    public bool TalkActiveSelf()
    {
        return _TalkWindow.activeSelf;
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkManager.GetTalk(id, talkIndex);
    }
    //Talk Manager

    //Quest Manager
    public void RequestOpen(string title, string description, QuestType type)
    {
        _RequestWindow.SetActive(true);
        _RequestWindow.GetComponent<RequestWindow>().requestSetting(title, description, type);
    }

    public void npcQuestSetting(NPCData npc)
    {
        questManager.npcQuest(npc);
    }

    public bool QuestBtnSetActvie(int id, int talkIndex)
    {
        return talkManager.QuestBtnSetActvie(id, talkIndex);
    }
    //Quest Manager

    //Talk + Quest
    public void QuestExit()
    {
        _RequestWindow.SetActive(false);
    }
    #endregion [Manager Methods]

    #region [Skill & Hot bar key Methods]

    public void Qskill_CoolTime(float Initcool, float cooltime)
    {
        SkillText_Q.gameObject.SetActive(true);
        Skillbar_Q.fillAmount = 1 - (cooltime / Initcool);
        int cool = (int)cooltime;
        if (cool > 60)
            SkillText_Q.text = cool / 60 + "m";
        else
            SkillText_Q.text = cool.ToString();

        if (cooltime <= 1)
            SkillText_Q.gameObject.SetActive(false);

    }

    public void Eskill_CoolTime(float Initcool, float cooltime)
    {
        SkillText_E.gameObject.SetActive(true);
        Skillbar_E.fillAmount = 1 - (cooltime / Initcool);
        int cool = (int)cooltime;
        if (cool > 60)
            SkillText_E.text = cool / 60 + "m";
        else
            SkillText_E.text = cool.ToString();

        if (cooltime <= 1)
            SkillText_E.gameObject.SetActive(false);

    }

    public void Rskill_CoolTime(float Initcool, float cooltime)
    {
        SkillText_R.gameObject.SetActive(true);
        Skillbar_R.fillAmount = 1 - (cooltime / Initcool);
        int cool = (int)cooltime;
        if (cool > 60)
            SkillText_R.text = cool / 60 + "m";
        else
            SkillText_R.text = cool.ToString();

        if (cooltime <= 1)
            SkillText_R.gameObject.SetActive(false);
    }

    #endregion [Skill & Hot bar key Methods]

    #region Couroutine Methods
    IEnumerator MonsterReSpwan()
    {
        int _positionNum = 0;
        for (int i = 0; i < MonsterManager.Instance._genCheck.Length; i++)
        {
            if (!MonsterManager.Instance._genCheck[i])
            {
                _positionNum = i;
                break;
            }
        }
        yield return new WaitForSeconds(2f);
        MonsterManager.Instance.CreateMonster(_positionNum);
        //���� ����
    } // ���͸� �ڵ����� ��ȯ�����ֱ� ���� �ڷ�ƾ
    #endregion
}
