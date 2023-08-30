using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class IngameManager : SingletonMonobehaviour<IngameManager>
{
    public GameObject[] SwordWeapons;

    //정보 변수
    MapType _currentMap;
    int _spawnMAX; // 최대 소환할 수 있는 몬스터들 수
    public int _spawnNum;
    Coroutine _runningCoroutine;

    private void Start()
    {
        ChangeMapFromMapType(MapType.Stage1); // 임시로 Stage1로 설정
        //플레이어를 불러올때 위치를 보고 스테이지를 불러올 거임.
    }

    private void Update()
    {
        MapState();
    }

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
        //맵이 변경되도록 구현 addtive.
        _spawnMAX = _spawnNum = MonsterManager.Instance._genPosition.Length;
        _currentMap = _type;
    }

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
        //몬스터 생성
    }
    #endregion
}
