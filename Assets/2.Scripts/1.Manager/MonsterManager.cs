using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    [Header("Edit Param")]
    [SerializeField] GameObject[] _monsterPrefab; //등록할 몬스터
    [SerializeField] GameObject _genPositionPrefab; //몬스터가 소환될 위치
    [SerializeField] int _maxNumber = 8;

    GameObjectPool<MonsterController>[] _monsterPool;
    public SpawnPos[] _genPosition; // 소환될 위치들
    public bool[] _genCheck;


    void Start()
    {
        SetInitMonster();
    }

    public void SetInitMonster()
    {
        _genPosition = _genPositionPrefab.GetComponentsInChildren<SpawnPos>();
        _genCheck = new bool[_genPosition.Length];

        _monsterPool = new GameObjectPool<MonsterController>[_monsterPrefab.Length];

        for (int i = 0; i < _monsterPrefab.Length; i++)
        {
            _monsterPool[i] = new GameObjectPool<MonsterController>(_maxNumber, () =>
            {
                var obj = Instantiate(_monsterPrefab[i]);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                var _monster = obj.GetComponent<MonsterController>();
                //mon.SetMonster(m_uiCamera, m_hudPool);
                return _monster;
            });
        }

        InitCreateMonster();
    }

    public void InitCreateMonster()
    {
        for(int i = 0; i < _genPosition.Length; i++)
        {
            CreateMonster(i);
        }
    }

    public void CreateMonster(int _genNum)
    {
        int _monNum = _genPosition[_genNum]._MONNUM;
        var _monster = _monsterPool[_monNum].Get();
        _monster.InitMonster(_genPosition[_genNum]);
        _genCheck[_genNum] = true;
    }

    public void RemoveMonster(MonsterController mon)
    {
        IngameManager.Instance._spawnNum -= 1;
        mon.gameObject.SetActive(false);
        _monsterPool[mon._monNumber].Set(mon);
    }

}
