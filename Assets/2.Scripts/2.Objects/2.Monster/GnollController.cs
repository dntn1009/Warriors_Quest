using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class GnollController : MonsterController
{
    bool _overDrive;

    private void Awake()
    {
        _overDrive = false;
        _isHit = false;
        _isCombo = 0;
        _idleTime = 0f;
        AnimatorResetting();
        _genPosition = transform.position;
    }
    private void Start()
    {
        InitializeSet();
        ChangeAniFromType(AnyType.IDLE);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        BehaviourProcess();
    }

    override public void BehaviourProcess()
    {
        switch (_state)
        {
            case BehaviourState.IDLE:
                _idleDuration += Time.deltaTime;
                if (_idleTime <= _idleDuration)
                {
                    if (_isHit) // 공격 받았으면
                    {

                        SetState(BehaviourState.CHASE);
                        ChangeAniFromType(AnyType.RUN, 1f);
                        _navAgent.isStopped = false;

                    }
                    else
                    {
                        SetState(BehaviourState.PATROL);
                        ChangeAniFromType(AnyType.WALK, 0f);
                        _navAgent.destination = GetRandomPos();
                        _navAgent.isStopped = false;
                    }
                }
                break;
            case BehaviourState.PATROL:
                if (_isHit) // 공격 받으면
                {
                    SetIdleDuration(0.5f);
                    return;
                }
                else if (_navAgent.remainingDistance <= 0)
                {
                    SetIdleDuration(1f);
                    ChangeAniFromType(AnyType.IDLE);
                }
                break;
            case BehaviourState.CHASE:
                if (_isZone)
                {
                    _isHit = false;
                    SetIdleDuration(1f);
                    ChangeAniFromType(AnyType.IDLE);
                    return;
                }
                else
                {
                    if (FindTarget(_player.transform, _attackPos))
                    {
                        _navAgent.isStopped = true;
                        if (_isCombo == 0)
                        {
                            SetState(BehaviourState.ATTACK1);
                            ChangeAniFromType(AnyType.ATTACK1);
                        }
                        else if (_isCombo == 1)
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
                _attackForward = _player.transform.position - transform.position;
                _attackForward.y = 0f;
                transform.forward = _attackForward;
                break;
            case BehaviourState.ATTACK2:
                _attackForward = _player.transform.position - transform.position;
                _attackForward.y = 0f;
                transform.forward = _attackForward;
                break;
            case BehaviourState.HIT:
                break;
            case BehaviourState.DEATH:
                _isHit = false;
                _navAgent.isStopped = true;
                Invoke("DeathMonster", 2.5f);
                break;
            case BehaviourState.OVERDRIVE:
                break;
        }
    }

    override public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath || _state == BehaviourState.OVERDRIVE) return;

        _isHit = true; // 공격시 따라가게 하기위함 (CHASE)

        HP -= Mathf.CeilToInt(damage);
        if (HP <= 0f)
        {
            HP = 0;
            _overDrive = false;
            SetState(BehaviourState.DEATH);
            ChangeAniFromType(AnyType.DEATH);
            AnimEvent_deadSounds(1);
            monsterReward(_player);
            return;
        }
        else if(HP <= HPMAX / 3 && !_overDrive)
        {
            _overDrive = true;
            SetState(BehaviourState.OVERDRIVE);
            ChangeAniFromType(AnyType.OVERDRIVE);
            return;
        }

        _hudObjcet.UpdateHPBar(HP, HPMAX);

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Normal)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.white); //데미지  UI 표시

        if (attackType == AttackType.Critical)
        {
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.red); //데미지  UI 표시
            SetState(BehaviourState.HIT);
            ChangeAniFromType(AnyType.HIT);
            _navAgent.isStopped = true;
        }
    }

    override public void AnimEvent_Attack(int _areaNum)
    {
        if (_areaNum == 0)
        {
            if (_AttackAreUnitFind.Length <= 1)
                _isCombo = 0;
            else
                _isCombo = 1;
        }
        else
            _isCombo = 0;

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
                if (_overDrive) damage += damage / 2;
                player.SetDemage(type, damage);
                Debug.Log("몬스터의 공격 : " + damage);
                if (type == AttackType.Dodge) return;
                else if (type == AttackType.Normal)
                {
                    var effect = Instantiate(_fxHitPrefab[0]);
                    effect.transform.position = dummy.transform.position;
                }
                else if (type == AttackType.Critical)
                {
                    var effect = Instantiate(_fxHitPrefab[1]);
                    effect.transform.position = dummy.transform.position;
                }
            }
        }
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
            _AttackAreUnitFind[i].UnitList.RemoveAll(obj => obj.GetComponent<PlayerController>()._isDeath);
    }

    public void AnimEvent_OverDrive()
    {
        SetIdleDuration(0.5f);
        ChangeAniFromType(AnyType.IDLE);
        AudioManager.Instance.monsterPlay(AudioManager.Instance.Growling);
    }
}
