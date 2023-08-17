using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MonsterController : MonsterStat
{
    [Header("Edit Param")]
    [SerializeField] float _limitWidth = 8;
    [SerializeField] float _limitFrontBack = 8;
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object

    //참조 변수
    // _navAgent - MonsterAnimController Protected
    BehaviourState _state;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;

    //정보 변수
    Vector3 _genPosition;
    float _idleDuration;
    float _idleTime;

    Transform _playerPos;

    public bool _isDeath { get { if (_state == BehaviourState.DEATH)
                return true;
            return false; } }

    bool _isHit;

    private void Awake()
    {
        _isHit = false;
        _idleTime = 0f;
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

        _AttackRngs = _AttackAreaPrefab.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < _AttackRngs.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackRngs[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
            _AttackRngs[i].enabled = false;
        }
    }

    public void BehaviourProcess()
    {
        // _isHit True => Chase, _isHit false => Patrol
        switch (_state)
        {
            case BehaviourState.IDLE:
                SetHitChase();

                _idleDuration += Time.deltaTime;
                if (_idleTime <= _idleDuration)
                {
                    if (_navAgent.remainingDistance <= 0)
                    {
                        ChangeAniFromType(AnyType.WALK);
                        _navAgent.destination = GetRandomPos();
                        SetState(BehaviourState.PATROL);
                    }
                }
                break;
            case BehaviourState.PATROL:
                SetHitChase();

                if (_navAgent.remainingDistance <= 0)
                {
                    SetIdleDuration(1f);
                }

                break;
            case BehaviourState.CHASE:
                CheckZoneSetIdle();

                //만약 findtarget에 되면 attack으로 넘어가고
                //else
                _navAgent.destination = _playerPos.position;

                //CHASE면 PLAYER 쫒아다녀야함.
                //현재 포지션에 GENPOSITION안에 네모난 툴에 벗어나면 원래 포지션으로 이동하기.
                //일반 몹
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:
                _playerPos = null;
                _isHit = false;
                break;
        }
    }
    void SetState(BehaviourState state)
    {
        _state = state;
    }

    #endregion [Character Setting Methods]

    #region [Behaviour Methods]
    public void SetIdleDuration(float duration) // IDLE로 있는 Term 발생
    {
        _idleTime = duration;
        _idleDuration = 0f;
        SetState(BehaviourState.IDLE);
    }

    public void SetHitChase()
    {
        if (_isHit)
        {
            SetState(BehaviourState.CHASE);
            ChangeAniFromType(AnyType.RUN);
        }
    } // Demage를 입으면 _playerpos를 등록하여 쫒아다니도록 함.
    public void CheckZoneSetIdle()
    {
        if (this.transform.position.x <= _genPosition.x + _limitWidth
            && this.transform.position.x >= _genPosition.x - _limitWidth
            && this.transform.position.y <= _genPosition.y + _limitFrontBack
            && this.transform.position.y >= _genPosition.y - _limitFrontBack)
        {
            _isHit = false;
            _playerPos = null;
            ChangeAniFromType(AnyType.WALK);
            SetIdleDuration(1f);
        }
    } // Chase 과정 중에 구역을 벗어나면 원래자리로 돌아가도록 함.
    #endregion [Animation Methods]

    #region [Move Methods]
    Vector3 GetRandomPos()
    {
        float px = Random.Range(-_limitWidth, _limitWidth);
        float pz = Random.Range(-_limitFrontBack, _limitFrontBack);

        return _genPosition + new Vector3(px, 0, pz);
    }

    #endregion [Move Methods]

    #region [Attack & Demage Methods]
    bool FindTarget(Transform target, float distance)
    {
        var dir = target.position - transform.position;
        dir.y = 0f;
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 1f, dir.normalized * distance, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 1f, dir.normalized, out hit, distance, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player")))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    public void SetDemage(Transform playerPos, AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _isHit = true; // 공격시 따라가게 하기위함 (CHASE)
        _playerPos = playerPos;
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
