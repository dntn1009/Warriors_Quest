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
    BehaviourState _state;
    //정보 변수
    Vector3 _genPosition;

    public bool _isDeath { get { if (_state == BehaviourState.DEATH)
                return true;
            return false;} }

    private void Awake()
    {
        AnimatorResetting();
        _genPosition = transform.position;
    }
    private void Start()
    {
        InitializeSet();
        ChangeAniFromType(AnyType.RUN);
    }

    private void Update()
    {
        BehaviourProcess();
    }

    #region [Character Setting Methods]
    public void InitializeSet()
    {
        _state = BehaviourState.IDLE;
        //임시
        _stat = new Stat(400, 400, 0, 0, 38, 0, 80, 7, 15, 10, 55);
        // Monster Stat Setting 구현해야함.

        /*_AttackRngs = _AttackAreaPrefab.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < _AttackRngs.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackRngs[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
            _AttackRngs[i].enabled = false;
        }*/
    }

    public void BehaviourProcess()
    {
        switch (_state)
        {
            case BehaviourState.IDLE:
                if (_navAgent.remainingDistance <= 0)
                    _navAgent.destination = GetRandomPos();
                break;
            case BehaviourState.PATROL:
                break;
            case BehaviourState.CHASE:
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:
                ChangeAniFromType(AnyType.DEATH);
                break;

        }
    }

    #endregion [Character Setting Methods]

    #region [Move Methods]
    Vector3 GetRandomPos()
    {
        float px = Random.Range(-_limitWidth, _limitWidth);
        float pz = Random.Range(-_limitFrontBack, _limitFrontBack);

        return _genPosition + new Vector3(px, 0, pz);
    }

    #endregion [Move Methods]

    #region [Attack & Demage Methods]
    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _stat.HP -= Mathf.CeilToInt(damage);
        //m_hudCtr.DisplayDamage(attackType, damage, playInfo.hp / (float)playInfo.hpMax);
        //데미지  UI 표시

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Critical)
            ChangeAniFromType(AnyType.HIT, false);

        if (_stat.HP <= 0f)
        {
            _state = BehaviourState.DEATH;
        }

    }
    #endregion [Attack & Demage Methods]
}
