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
        // _isHit True => Chase, _isHit false => Patrol
        switch (_state)
        {
            case BehaviourState.IDLE:
                if (_navAgent.remainingDistance <= 0)
                {
                    ChangeAniFromType(AnyType.WALK);
                    _navAgent.destination = GetRandomPos();
                    _state = BehaviourState.PATROL;
                }

                break;
            case BehaviourState.PATROL:
                if (_navAgent.remainingDistance <= 0)
                {
                    //Idle Deceision을 만들어서 랜덤으로 아이들 가만히 있게 하거나, 움직이도록 구현해야함.
                    //피격시에 Idle & Patrol에서 Chase로 넘어가고 Chase <-> Attack 연계 되도록 해야함.
                    //Chase에서 Genposition 안에 있으면 계속 Chase 아니면 PATROL로 넘어가게 구현.
                    //일반 몹
                    _state = BehaviourState.IDLE;
                    //_navAgent.destination = GetRandomPos();
                }

                    break;
            case BehaviourState.CHASE:
                ChangeAniFromType(AnyType.RUN);
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:

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
            ChangeAniFromType(AnyType.DEATH);
            _state = BehaviourState.DEATH;
        }
    }
    #endregion [Attack & Demage Methods]
}
