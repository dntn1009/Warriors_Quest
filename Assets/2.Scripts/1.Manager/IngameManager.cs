using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class IngameManager : SingletonMonobehaviour<IngameManager>
{
    public GameObject[] SwordWeapons;
    [SerializeField] GameObject DamageManager;
    [SerializeField] Canvas DamageObject;

    //���� ����
    MapType _currentMap;
    int _spawnMAX; // �ִ� ��ȯ�� �� �ִ� ���͵� ��
    public int _spawnNum;
    Coroutine _runningCoroutine;

    private void Start()
    {
        ChangeMapFromMapType(MapType.Stage1); // �ӽ÷� Stage1�� ����
        //�÷��̾ �ҷ��ö� ��ġ�� ���� ���������� �ҷ��� ����.
    }

    private void Update()
    {
        MapState();
    }

    #region [Map & Spawn Methods]
    public void MapState()
    {
        if (_runningCoroutine != null)
        {
            return;

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


    #region [Damage Methods]

    public void ShowDamage()
    {

    }

    public void HideDamage()
    {

    }

    #endregion [Damage Methods]
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
