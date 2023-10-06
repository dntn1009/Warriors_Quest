using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using TMPro;

public class IngameManager : SingletonMonobehaviour<IngameManager>
{
    [SerializeField] Transform DamageManager;
    [SerializeField] GameObject DamageObject;
    [SerializeField] GameObject Inventory;

    //���� ����
    MapType _currentMap;
    int _spawnMAX; // �ִ� ��ȯ�� �� �ִ� ���͵� ��
    public int _spawnNum;
    Coroutine _runningCoroutine;
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

        if(Input.GetKeyDown(KeyCode.I))
        {
            InventoryOpen();   
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if(!Inventory.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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

    #endregion [Damage UI Methods]

    #region [Inventory UI Methods]
    public void InventoryOpen()
    {
        if (Inventory.activeSelf)
        {
            Inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }


    #endregion [Inventory UI Methods]

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
