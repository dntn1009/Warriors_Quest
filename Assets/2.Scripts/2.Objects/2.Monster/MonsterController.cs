using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MonsterController : MonsterStat
{
    [Header("Edit Param")]
    [SerializeField] float _limitWidth = 8;
    [SerializeField] float _limitFrontBack = 8;
    [SerializeField] float _attackPos = 0.3f; // ���� ����
    [SerializeField] float _preemptivePos = 1.2f; // ���� ���� ����
    [SerializeField] GameObject _AttackAreaPrefab; // ���� ������ �ʿ��� Collider ���� Object
    [SerializeField] bool _isPreemptive; // ���� �����ϴ� �������� �ƴ��� üũ
    [SerializeField] HudController _hudObjcet;
    //���� ����
    // _navAgent - MonsterAnimController Protected
    BehaviourState _state; // ���� ����
    AttackAreUnitFind[] _AttackAreUnitFind; // AttackAreUnitFinds;

    //���� ����
    Vector3 _genPosition;
    Vector3 _attackForward;
    float _idleDuration;
    float _idleTime;

    float _attackDuration;
    float _attackTime;

    PlayerController _player;
    int _monNum;

    public bool _isDeath { get { if (_state == BehaviourState.DEATH)
                                    return true;
                                  return false; } }

    public bool _isCriticalHit { get { if (GetAnimState() == AnyType.HIT)
                return true;
            return false;} }

    public int _monNumber { get { return _monNum; } set { _monNum = value; } }

    bool _isHit;
    bool _isAttack;
    int _isCombo;

    private void Awake()
    {
        _isHit = false;
        _isAttack = false;
        _isCombo = 0;
        _idleTime = 0f;
        _attackTime = 0f;
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
        //�ӽ�
        _stat = new Stat(string.Empty, 400, 400, 0, 0, 1, 38, 0, 80, 7, 15, 10, 55);
        // Monster Stat Setting �����ؾ���.

        _AttackAreUnitFind = _AttackAreaPrefab.GetComponentsInChildren<AttackAreUnitFind>();
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackAreUnitFind[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
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
                    ChangeAniFromType(AnyType.IDLE);
                }

                break;
            case BehaviourState.CHASE:
                CheckZoneSetIdle();

                if (!_isCriticalHit)
                {
                    if (FindTarget(_player.transform, _attackPos))
                    {
                        _isAttack = true;
                        _navAgent.isStopped = true;

                        if(_isCombo == 0)
                        {
                            SetState(BehaviourState.ATTACK1);
                            ChangeAniFromType(AnyType.ATTACK1);
                        }
                        else if(_isCombo == 1)
                        {
                            SetState(BehaviourState.ATTACK2);
                            ChangeAniFromType(AnyType.ATTACK2);
                        }
                    }
                    else
                        _navAgent.destination = _player.transform.position;
                }
                break;
            case BehaviourState.ATTACK1:
                if (GetAnimState() == AnyType.HIT)
                    return;

                if (!_isAttack)
                {
                    _attackDuration += Time.deltaTime;
                    if (_attackTime <= _attackDuration)
                    {
                        if (_AttackAreUnitFind.Length <= 1)
                            _isCombo = 0;
                        else
                            _isCombo = 1;
                        SetState(BehaviourState.CHASE);
                        ChangeAniFromType(AnyType.RUN);
                        _navAgent.isStopped = false;
                    }
                }
                else
                {
                    _attackForward = _player.transform.position - transform.position;
                    _attackForward.y = 0f;
                    transform.forward = _attackForward;
                }
                break;
            case BehaviourState.ATTACK2:
                if (GetAnimState() == AnyType.HIT)
                    return;

                if (!_isAttack)
                {
                    _attackDuration += Time.deltaTime;
                    if(_attackTime <= _attackDuration)
                    {
                        _isCombo = 0;
                        SetState(BehaviourState.CHASE);
                        ChangeAniFromType(AnyType.RUN);
                        _navAgent.isStopped = false;
                    }
                }
                else
                {
                    _attackForward = _player.transform.position - transform.position;
                    _attackForward.y = 0f;
                    transform.forward = _attackForward;
                }
                break;
            case BehaviourState.DEATH:
                _isHit = false;
                _navAgent.isStopped = true;
                Invoke("DeathMonster", 2.5f);
                break;
        }
    }
    void SetState(BehaviourState state)
    {
        _state = state;
    }

    #endregion [Character Setting Methods]

    #region [Behaviour Methods]
    public void SetIdleDuration(float duration) // IDLE�� �ִ� Term �߻�
    {
        _idleTime = duration;
        _idleDuration = 0f;
        SetState(BehaviourState.IDLE);
    }

    public void SetAttackDuration(float duration) // IDLE�� �ִ� Term �߻�
    {
        _attackTime = duration;
        _attackDuration = 0f;
    }

    public void SetHitChase()
    {
        if (_isPreemptive) // �������� ������ ���
        {
            if (this.transform.position.x > _genPosition.x + _limitWidth
            || this.transform.position.x < _genPosition.x - _limitWidth
            || this.transform.position.y > _genPosition.y + _limitFrontBack
            || this.transform.position.y < _genPosition.y - _limitFrontBack) // ������ ���� �ٱ��̶��
            {
                _isHit = false;
                ChangeAniFromType(AnyType.WALK);
                SetIdleDuration(1f);
            } // �񼱰����� �ٲ��ְ�
            else if (FindTarget(_player.transform, _preemptivePos)) // ������ �����ȿ� ������ ���̸�ŭ RayCast�� ������
            {
                _isHit = true;
                SetState(BehaviourState.CHASE);
                ChangeAniFromType(AnyType.RUN);
            }
        }
        else
        {
            if (_isHit)
            {
                SetState(BehaviourState.CHASE);
                ChangeAniFromType(AnyType.RUN);
            }
        }

    } // Demage�� ������ _playerpos�� ����Ͽ� �i�ƴٴϵ��� ��.
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
            _hudObjcet.HideHPBar();
        }
    } // Chase ���� �߿� ������ ����� �����ڸ��� ���ư����� ��.
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
        //+ Vector3.up * 1f
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, dir.normalized * distance, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, dir.normalized, out hit, distance, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player")))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;

        _isHit = true; // ���ݽ� ���󰡰� �ϱ����� (CHASE)
        _stat.HP -= Mathf.CeilToInt(damage);
        _hudObjcet.UpdateHPBar(_stat.HP, _stat.MAXHP);

        if (attackType == AttackType.Dodge) return;

        if(attackType == AttackType.Normal)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.white); //������  UI ǥ��

        if (attackType == AttackType.Critical)
        {
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.red); //������  UI ǥ��
            ChangeAniFromType(AnyType.HIT);
            _navAgent.isStopped = true;
        }

        if (_stat.HP <= 0f)
        {
            ChangeAniFromType(AnyType.DEATH);
            _state = BehaviourState.DEATH;
        }
    }
    //Attack Methods

    //AnimEvent Methods

    public void AnimEvent_Hit()
    {
        if (_state == BehaviourState.ATTACK1)
            ChangeAniFromType(AnyType.ATTACK1);
        else if (_state == BehaviourState.ATTACK2)
            ChangeAniFromType(AnyType.ATTACK2);
        else
        {
            ChangeAniFromType(AnyType.RUN);
            _navAgent.isStopped = false;
        }
    }
    public void AnimEvent_Attack(int _areaNum)
    {
        float damage = 0f;
        var unitList = _AttackAreUnitFind[_areaNum].UnitList;
        for (int i = 0; i < unitList.Count; i++)
        {
            var player = unitList[i].GetComponent<PlayerController>();
            var dummy = Util.FindChildObject(unitList[i], "Player_Hit");
            if (dummy != null && player != null)
            {
                if (player._isDeath) continue;
                AttackType type = Util.AttackProcess(this, player, out damage);
                player.SetDemage(type, damage);
                Debug.Log("������ ���� : " + damage);
                if (type == AttackType.Dodge) return;
            }
        }
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
            _AttackAreUnitFind[i].UnitList.RemoveAll(obj => obj.GetComponent<PlayerController>()._isDeath);
    }
    public void AnimEvent_AttackFinished()
    {
        _isAttack = false;
        SetAttackDuration(1.5f);
        ChangeAniFromType(AnyType.IDLE);
    }
    //AnimEvent Methods

    #endregion [Attack & Demage Methods]

    #region [MonsterManager Script Methods]

    public void InitMonster(SpawnPos _genTransform)
    {
        //HP ReSetting
        _hudObjcet.InitHPBar();
        //Monster Spawn ����
        _monNum = _genTransform._MONNUM;
        transform.position = _genTransform.transform.position;
        _genPosition = _genTransform.transform.position;
        gameObject.SetActive(true);
    }

    public void DeathMonster()
    {
        MonsterManager.Instance.RemoveMonster(this);
    }

    #endregion [MonsterManager Script Methods]
}
