using System.Collections;
using UnityEngine;
using DefineHelper;

public class MushRoomController : MonsterController
{
    [Header("Edit Param")]
    [SerializeField] float _limitWidth = 8;
    [SerializeField] float _limitFrontBack = 8;
    [SerializeField] protected float _attackPos = 0.3f; // 공격 범위
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object
    [SerializeField] protected GameObject[] _fxHitPrefab;
    [SerializeField] protected HudController _hudObjcet;
    //참조 변수
    // _navAgent - MonsterAnimController Protected
    protected AttackAreUnitFind[] _AttackAreUnitFind; // AttackAreUnitFinds;

    //정보 변수
    protected Vector3 _genPosition;
    protected Vector3 _attackForward;
    protected float _idleDuration;
    protected float _idleTime;

    protected PlayerController _player;
    public bool _isZone
    {
        get
        {
            if (this.transform.position.x > _genPosition.x + _limitWidth
                || this.transform.position.x < _genPosition.x - _limitWidth
                || this.transform.position.z > _genPosition.z + _limitFrontBack
                || this.transform.position.z < _genPosition.z - _limitFrontBack)
                return true;
            return false;
        }
    }// 공격 및 패트롤 범위

    protected bool _isHit; // Chase - Patrol 기준이 되는 Bool
    protected int _isCombo; // 공격 패턴 구분

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        Init_HPSetting();
        _isHit = false;
        _isCombo = 0;
        _idleTime = 0f;
        _genPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _AttackAreUnitFind = _AttackAreaPrefab.GetComponentsInChildren<AttackAreUnitFind>();
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackAreUnitFind[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
        }
    }

    #region [State Methods]
    public override void OnStateEnter(BehaviourState state)
    {
        switch (state)
        {
            case BehaviourState.IDLE:
                ChangeAniFromType(AnyType.IDLE);
                _idleDuration = 0f;
                break;
            case BehaviourState.PATROL:
                ChangeAniFromType(AnyType.WALK, 0f);
                _navAgent.destination = GetRandomPos();
                break;
            case BehaviourState.CHASE:
                ChangeAniFromType(AnyType.RUN, 1f);
                break;
            case BehaviourState.ATTACK1:
                ChangeAniFromType(AnyType.ATTACK1);
                _attackForward = _player.transform.position - transform.position;
                _attackForward.y = 0f;
                transform.forward = _attackForward;
                break;
            case BehaviourState.HIT:
                ChangeAniFromType(AnyType.HIT);
                break;
            case BehaviourState.DEATH:
                ChangeAniFromType(AnyType.DEATH);
                AnimEvent_attackSounds();
                _isHit = false;
                _navAgent.isStopped = true;
                Invoke("DeathMonster", 2.5f);
                break;
        }
    }//State 시작

    public override void OnStateExit(BehaviourState state)
    {
        switch (state)
        {
            case BehaviourState.IDLE:
                _navAgent.isStopped = false;
                break;
            case BehaviourState.PATROL:
                break;
            case BehaviourState.CHASE:
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:
                break;
        }
    }//State 종료

    public override void StateUpdate(BehaviourState state)
    {
        switch (state)
        {
            case BehaviourState.IDLE:
                _idleDuration += Time.deltaTime;
                if (_idleTime <= _idleDuration)
                {
                    if (_isHit) // 공격 받았으면
                        ChangeState(BehaviourState.CHASE);
                    else
                        ChangeState(BehaviourState.PATROL);
                }
                break;
            case BehaviourState.PATROL:
                if (_isHit) // 공격 받으면
                {
                    SetIdleDuration(0.5f);
                    return;
                }
                
                if (_navAgent.remainingDistance <= 0)
                    SetIdleDuration(1f);
                break;
            case BehaviourState.CHASE:
                if (_isZone)
                {
                    _isHit = false;
                    SetIdleDuration(1f);
                    return;
                }
                else
                {
                    if (FindTarget(_player.transform, _attackPos))
                    {
                        _navAgent.isStopped = true;
                        if (_isCombo == 0)
                            ChangeState(BehaviourState.ATTACK1);
                    }
                    else
                        _navAgent.destination = _player.transform.position;
                }
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:
                break;
        }
    }//State Update

    public override void StateFixedUpdate(BehaviourState state)
    {
        switch (state)
        {
            case BehaviourState.IDLE:

                break;
            case BehaviourState.PATROL:
                break;
            case BehaviourState.CHASE:
                break;
            case BehaviourState.ATTACK1:
                break;
            case BehaviourState.DEATH:
                break;
        }
    }//State FixedUpdate

    #endregion [State Methods]

    #region [BehaviourState Methods]
    public void SetIdleDuration(float duration) // IDLE로 있는 Term 발생
    {
        _navAgent.isStopped = true;
        _idleTime = duration;
        ChangeState(BehaviourState.IDLE);
    }
    #endregion [BehaviourState Methods]

    #region [Attack & Demage Methods]

    //Attack Method
    protected bool FindTarget(Transform target, float distance)
    {
        var dir = target.position - transform.position;
        dir.y = 0f;
        RaycastHit hit;
        //+ Vector3.up * 1f
        Debug.DrawRay(transform.position + Vector3.up * 1.1f, dir.normalized * distance, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 1.1f, dir.normalized, out hit, distance, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player")))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }
        return false;
    }

    public override void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;

        _isHit = true; // 공격시 따라가게 하기위함 (CHASE)
        HP -= Mathf.CeilToInt(damage);
        if (HP <= 0f)
        {
            HP = 0;
            ChangeState(BehaviourState.DEATH);
            monsterReward(_player);
            return;
        }

        _hudObjcet.UpdateHPBar(HP, HPMAX);

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Normal)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.white); //데미지  UI 표시

        if (attackType == AttackType.Critical)
        {
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Monster_Hit").transform.position, damage.ToString(), Color.red); //데미지  UI 표시
            ChangeState(BehaviourState.HIT);
            ChangeAniFromType(AnyType.HIT);
            _navAgent.isStopped = true;
        }
    }

    public void monsterReward(PlayerController _player)
    {
        _player._stat.EXP += EXP;
        _player._stat.GOLD += GOLD;
        //_player.QuestTypeKill(this);
        _player.LevelUP();
        ItemDrop();
        IngameManager.Instance.SetGetInfoText("Exp + " + EXP);
        IngameManager.Instance.SetGetInfoText("GOLD + " + GOLD);
    }
    //Attack Methods
    #endregion [Attack & Demage Methods]

    #region [Move Methods]
    protected Vector3 GetRandomPos()
    {
        float px = Random.Range(-_limitWidth, _limitWidth);
        float pz = Random.Range(-_limitFrontBack, _limitFrontBack);

        return _genPosition + new Vector3(px, 0, pz);
    }

    #endregion [Move Methods]

    #region [Inventory Item Methods]

    public void ItemDrop()
    {
        if (CODE == 100)
            Inventory.Singleton.GetDropItem(0);
        else if (CODE == 101)
            Inventory.Singleton.GetDropItem(1);
        else if (CODE == 102)
            Inventory.Singleton.GetRandomDropItem(15, 5);

        Inventory.Singleton.GetRandomDropItem(2, 10);
        Inventory.Singleton.GetRandomDropItem(3, 10);
    }

    #endregion [Inventory Item Methods]

    #region [Abstract AnimEvent Methods]
    public override void AnimEvent_Attack(int _areaNum)
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
    public override void AnimEvent_AttackFinished()
    {
        SetIdleDuration(1.2f);
    }
    public override void AnimEvent_Hit()
    {
        SetIdleDuration(0.5f);
    }
    public override void AnimEvent_attackSounds()
    {
        AudioManager.Instance.monsterPlay(AudioManager.Instance.mush_slimeAttack);
    }
    public override void AnimEvent_deadSounds()
    {
        AudioManager.Instance.monsterPlay(AudioManager.Instance.mush_slimedead);
    }
    public override void AnimEvent_growling()
    {
        AudioManager.Instance.monsterPlay(AudioManager.Instance.Growling);
    }
    #endregion [Abstract AnimEvent Methods]

    #region [MonsterManager Script Methods]

    public override void InitMonster(SpawnPos _genTransform)
    {
        //HP ReSetting
        _hudObjcet.InitHPBar();
        _hudObjcet.InitName(NAME);
        //Monster Spawn 지역
        _monNum = _genTransform._MONNUM;
        transform.position = _genTransform.transform.position;
        _genPosition = _genTransform.transform.position;
        gameObject.SetActive(true);
    }

    public void DeathMonster()
    {
        //MonsterManager.Instance.RemoveMonster(this);
    }

    #endregion [MonsterManager Script Methods]
}
