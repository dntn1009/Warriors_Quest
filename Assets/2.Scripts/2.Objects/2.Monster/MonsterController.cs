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
    //���� ����
    // _navAgent - MonsterAnimController Protected
    BehaviourState _state; // ���� ����
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;

    //���� ����
    Vector3 _genPosition;
    float _idleDuration;
    float _idleTime;

    PlayerController _player;

    public bool _isDeath { get { if (_state == BehaviourState.DEATH)
                return true;
            return false; } }

    public bool _isCriticalHit { get { if (GetAnimState() == AnyType.HIT)
                return true;
            return false;} }

    bool _isHit;

    private void Awake()
    {
        _isHit = false;
        _idleTime = 0f;
        AnimatorResetting();
        _genPosition = transform.position;
        _navAgent.isStopped = false;
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
        _stat = new Stat(400, 400, 0, 0, 38, 0, 80, 7, 15, 10, 55);
        // Monster Stat Setting �����ؾ���.

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

                if (!_isCriticalHit)
                {
                    if (FindTarget(_player.transform, _attackPos))
                    {
                        _navAgent.isStopped = true;
                        SetState(BehaviourState.ATTACK1);
                        ChangeAniFromType(AnyType.ATTACK1);
                    }
                    else
                        _navAgent.destination = _player.transform.position;
                }
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.ATTACK2:

                break;
            case BehaviourState.DEATH:
                _isHit = false;
                _navAgent.isStopped = true;
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

    public void SetHitChase()
    {
        if (_isPreemptive)
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
            else if (FindTarget(_player.transform, _preemptivePos))
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
    public void SetAttack(GameObject player)
    {
        var _player = player.GetComponent<PlayerController>();
        var dummy = Util.FindChildObject(player, "Player_Hit");
        float demage = 0f;
        if (!_player._isDeath && dummy != null)
        {
            AttackType type = Util.AttackProcess(this, _player, out demage);
            _player.SetDemage(type, demage);
            if (type == AttackType.Dodge) return;
        }
    }

    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _isHit = true; // ���ݽ� ���󰡰� �ϱ����� (CHASE)
        _stat.HP -= Mathf.CeilToInt(damage);
        //m_hudCtr.DisplayDamage(attackType, damage, playInfo.hp / (float)playInfo.hpMax);
        //������  UI ǥ��

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Critical)
        {
            ChangeAniFromType(AnyType.HIT, false);
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
        ChangeAniFromType(AnyType.RUN);
        _navAgent.isStopped = false;
    }
    public void AnimEvent_Attack(int _areaNum)
    {
      /*  float damage = 0f;
        var unitList = _AttackAreUnitFind[_areaNum].UnitList;
        for (int i = 0; i < unitList.Count; i++)
        {
            var mon = unitList[i].GetComponent<MonsterController>();
            var dummy = Util.FindChildObject(unitList[i], "Monster_Hit");
            if (dummy != null && mon != null)
            {
                if (mon._isDeath) continue;
                AttackType type = Util.AttackProcess(this, mon, out damage);
                mon.SetDemage(type, damage);
                Debug.Log("������ : " + damage);
                if (type == AttackType.Dodge) return;
                else if (type == AttackType.Normal)
                {
                    var effect = Instantiate(_fxHitPrefab[0]);
                    effect.transform.position = dummy.transform.position;
                    effect.transform.rotation = Quaternion.FromToRotation(effect.transform.forward, (unitList[i].transform.position - transform.position).normalized);
                    Destroy(effect, 1.5f);
                }
                else if (type == AttackType.Critical)
                {
                    var effect = Instantiate(_fxHitPrefab[1]);
                    effect.transform.position = dummy.transform.position;
                    effect.transform.rotation = Quaternion.FromToRotation(effect.transform.forward, (unitList[i].transform.position - transform.position).normalized);
                    Destroy(effect, 1.5f);
                }
            }
        }
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
            _AttackAreUnitFind[i].UnitList.RemoveAll(obj => obj.GetComponent<MonsterController>()._isDeath);*/
    }
    public void AttackFinished()
    {
    /*    bool _isCombo = false;
        if (_isPressAttack) // ������ ������ _isPressAttack�� true�̱� ������ isCombo�� true�� ��.
            _isCombo = true;
        if (_keyBuffer.Count == 1) // keybuffer�� �̿��Ͽ� Ÿ�̹� �ȿ� ������ �׾ȿ� count�� 1�̸� �� �Ŀ� _isComobo�� true ��Ŵ
        {
            var key = _keyBuffer.Dequeue();
            if (key == KeyCode.Mouse0)
                _isCombo = true;
        }// 1�̿ܿ� ������ ���� �� ��� ���Դٸ� �ʱ�ȭ��Ŵ.
        else
        {
            ReleaseKeyBuffer();
        }
        if (_isCombo) // _isCombo ������ Ȯ���� �Ǹ� ComboIndex �� 1 ������Ű�� ComoboList�� �ִ� Attack�ִϸ��̼��� ���� ���� 1~3
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
