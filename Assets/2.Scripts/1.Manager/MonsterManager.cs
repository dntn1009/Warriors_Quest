using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonobehaviour<MonsterManager>
{
    [Header("Edit Param")]
    [SerializeField] GameObject[] _monsterPrefab; //등록할 몬스터
    [SerializeField] GameObject _genPositionPrefab; //몬스터가 소환될 위치

    GameObjectPool<MonsterController>[] _monsterPool;
    public Transform[] _genPosition; // 소환될 위치들
    public bool[] _genCheck;

    int _maxNumber = 6;


    void Start()
    {
        _genPosition = _genPositionPrefab.GetComponentsInChildren<Transform>();
        _genCheck = new bool[_genPosition.Length];
    }

    protected override void OnStart()
    {
        for(int i = 0; i < _monsterPrefab.Length; i++)
        {
            _monsterPool[i] = new GameObjectPool<MonsterController>(_maxNumber, () =>
            {
                var obj = Instantiate(_monsterPrefab[i]);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                var _monster = obj.GetComponent<MonsterController>();
                //mon.SetMonster(m_uiCamera, m_hudPool);
                return _monster;
            });
        }
    }

    public void CreateMonster(int _monNum, int _genNum)
    {
        var _monster = _monsterPool[_monNum].Get();
        _monster.InitMonster(_genPosition[_genNum]);
        _genCheck[_genNum] = true;

    }


    public void RemoveMonster(MonsterController mon)
    {
        mon.gameObject.SetActive(false);
        _monsterPool[1].Set(mon);
    }

}
