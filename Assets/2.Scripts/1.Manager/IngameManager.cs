using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngameManager : SingletonMonobehaviour<IngameManager>
{
    [Header("Demage")]
    [SerializeField] Transform DamageManager;
    [SerializeField] GameObject DamageObject;

    [Header("WindowUI")]
    [SerializeField] GameObject _Inventory;
    [SerializeField] Button _inventoryBtn;
    [SerializeField] GameObject _MapWindow;
    [SerializeField] GameObject _StatWindow;
    [SerializeField] GameObject _SkillWindow;
    [SerializeField] GameObject _MenuWindow;

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
        ChangeMapFromMapType(MapType.Stage1); // �ӽ÷� Stage1�� ����
        //�÷��̾ �ҷ��ö� ��ġ�� ���� ���������� �ҷ��� ����.
    }

    private void Update()
    {
        MapState();

        if(Input.GetKeyDown(KeyCode.I))//�κ��丮 ��ư
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
        if (Input.GetKeyDown(KeyCode.U))//�޴� ��ư
        {
            MenuOpen();
        }
        if(Input.GetKeyDown(KeyCode.M)) // ���� ��ư
        {
            MapOpen();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(!EventSystem.current.IsPointerOverGameObject())
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
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

    public void ChangeMapFromMapType(MapType _type)
    {
        //���� ����ǵ��� ���� addtive.
        _spawnMAX = _spawnNum = MonsterManager.Instance._genPosition.Length;
        _currentMap = _type;
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
    public void InventoryOpen()
    {
        if (_Inventory.activeSelf)
        {
            _Inventory.SetActive(false);
        }
        else
        {
            _Inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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

    #endregion [Window UI Methods]

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
        for(int i = 0; i < MonsterManager.Instance._genCheck.Length; i++)
        {
            if(!MonsterManager.Instance._genCheck[i])
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
