using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MonsterController : MonsterStat
{
    [Header("Edit Param")]
    [SerializeField] float _limitWidth = 8;
    [SerializeField] float _limitFrontBack = 8;
    

    //참조 변수
    // _navAgent - MonsterAnimController Protected

    //정보 변수
    Vector3 _genPosition;

    private void Awake()
    {
        AnimatorResetting();
        _genPosition = transform.position;
    }
    private void Start()
    {
        ChangeAniFromType(AnyType.RUN);
    }

    private void Update()
    {
        if(_navAgent.remainingDistance <= 0)
            _navAgent.destination = GetRandomPos();
    }

    Vector3 GetRandomPos()
    {
        float px = Random.Range(-_limitWidth, _limitWidth);
        float pz = Random.Range(-_limitFrontBack, _limitFrontBack);

        return _genPosition + new Vector3(px, 0, pz);
    }
}
