using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MonsterController : MonsterStat
{
    [Header("Edit Param")]
    [SerializeField] float _limitWidth = 8;
    [SerializeField] float _limitFrontBack = 8;
    [SerializeField] float _attackPos = 1.5f;
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object

    //참조 변수
    // _navAgent - MonsterAnimController Protected
    BehaviourState _state;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;

    //정보 변수
    Vector3 _genPosition;
    float _idleDuration;
    float _idleTime;

    PlayerController _player;

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
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

                if (FindTarget(_player.transform, _attackPos))
                {
                    SetState(BehaviourState.ATTACK1);
                }
                else
                {
                    _navAgent.destination = _player.transform.position;
                }
                break;
            case BehaviourState.ATTACK1:


                break;
            case BehaviourState.ATTACK2:

                break;
            case BehaviourState.DEATH:
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
        if (this.transform.position.x > _genPosition.x + _limitWidth
            || this.transform.position.x < _genPosition.x - _limitWidth
            || this.transform.position.y > _genPosition.y + _limitFrontBack
            || this.transform.position.y < _genPosition.y - _limitFrontBack)
        {
            _isHit = false;
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

    //Attack Method
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
    public void SetAttack(GameObject player)
    {
        var _player = player.GetComponent<PlayerController>();
        var dummy = Util.FindChildObject(player, "Player_Hit");
        float demage = 0f;
        Debug.Log("1번 확인");
        if (!_player._isDeath && dummy != null)
        {
            Debug.Log("들어감");
            AttackType type = Util.AttackProcess(this, _player, out demage);
            _player.SetDemage(type, demage);
            if (type == AttackType.Dodge) return;
        }
    }

    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _isHit = true; // 공격시 따라가게 하기위함 (CHASE)
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
    //Attack Methods

    //AnimEvent Methods
    public void AllOffAttackEnabled()
    {
        for (int i = 0; i < _AttackRngs.Length; i++)
        {
            _AttackRngs[i].enabled = false;
        }
    }
    public void OnAttackEnabled(int id)
    {
        _AttackRngs[id].enabled = true;
    }

    public void OffAttackEnabled(int id)
    {
        _AttackRngs[id].enabled = false;
    }

    public void AttackFinished()
    {
    /*    bool _isCombo = false;
        if (_isPressAttack) // 누르고 있으면 _isPressAttack이 true이기 때문에 isCombo가 true가 됌.
            _isCombo = true;
        if (_keyBuffer.Count == 1) // keybuffer를 이용하여 타이밍 안에 눌럿고 그안에 count가 1이면 뺀 후에 _isComobo를 true 시킴
        {
            var key = _keyBuffer.Dequeue();
            if (key == KeyCode.Mouse0)
                _isCombo = true;
        }// 1이외에 나머지 값이 더 계속 들어왔다면 초기화시킴.
        else
        {
            ReleaseKeyBuffer();
        }
        if (_isCombo) // _isCombo 누른게 확인이 되면 ComboIndex 를 1 증가시키고 ComoboList에 있는 Attack애니메이션을 고르기 위함 1~3
        {
            _comboIndex++;
            if (_comboIndex >= _comboList.Count)
                _comboIndex = 0;
            ChangeAniFromType(_comboList[_comboIndex]);
        }
        else
        {
            ChangeAniFromType(AnyType.IDLE);
            _comboIndex = 0;
        }*/
    }
    //AnimEvent Methods

    #endregion [Attack & Demage Methods]
}
