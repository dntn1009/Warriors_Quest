using DefineHelper;
using System.Collections;
using UnityEngine;

public class MonsterController : MonsterStat
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
    protected BehaviourState _state; // 현재 상태
    protected AttackAreUnitFind[] _AttackAreUnitFind; // AttackAreUnitFinds;

    //정보 변수
    protected Vector3 _genPosition;
    protected Vector3 _attackForward;
    protected float _idleDuration;
    protected float _idleTime;

    protected PlayerController _player;
    int _monNum;

    public bool _isDeath
    {
        get
        {
            if (_state == BehaviourState.DEATH)
                return true;
            return false;
        }
    } // Die 애니메이션
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
    public int _monNumber { get { return _monNum; } set { _monNum = value; } } // 현재 본인의  Monster Number;


    protected bool _isHit; // Chase - Patrol 기준이 되는 Bool
    protected int _isCombo; // 공격 패턴 구분

    private void Awake()
    {
        _isHit = false;
        _isCombo = 0;
        _idleTime = 0f;
        AnimatorResetting();
        _genPosition = transform.position;
    }
    private void Start()
    {
        InitializeSet();
        ChangeAniFromType(AnyType.IDLE); // 왜 RUN으로 해놨지?';;
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
        Init_HPSetting();
        // Monster Stat Setting 구현해야함.

        _AttackAreUnitFind = _AttackAreaPrefab.GetComponentsInChildren<AttackAreUnitFind>();
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackAreUnitFind[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
        }
    }

    public virtual void BehaviourProcess()
    {
        // _isHit True => Chase, _isHit false => Patrol

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
        }
    }
    protected void SetState(BehaviourState state)
    {
        _state = state;
    }

    #endregion [Character Setting Methods]

    #region [Behaviour Methods]
    public void SetIdleDuration(float duration) // IDLE로 있는 Term 발생
    {
        _navAgent.isStopped = true;
        _idleTime = duration;
        _idleDuration = 0f;
        SetState(BehaviourState.IDLE);
    }

    #endregion [Behaviour Methods]

    #region [Move Methods]
    protected Vector3 GetRandomPos()
    {
        float px = Random.Range(-_limitWidth, _limitWidth);
        float pz = Random.Range(-_limitFrontBack, _limitFrontBack);

        return _genPosition + new Vector3(px, 0, pz);
    }

    #endregion [Move Methods]

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

    virtual public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;

        _isHit = true; // 공격시 따라가게 하기위함 (CHASE)
        HP -= Mathf.CeilToInt(damage);
        if (HP <= 0f)
        {
            HP = 0;
            SetState(BehaviourState.DEATH);
            ChangeAniFromType(AnyType.DEATH);
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
            SetState(BehaviourState.HIT);
            ChangeAniFromType(AnyType.HIT);
            _navAgent.isStopped = true;
        }
    }

    public void monsterReward(PlayerController _player)
    {
        _player._stat.EXP += EXP;
        _player._stat.GOLD += GOLD;
        _player.QuestTypeKill(this);
        _player.LevelUP();
        ItemDrop();
        IngameManager.Instance.SetGetInfoText("Exp + " + EXP);
        IngameManager.Instance.SetGetInfoText("GOLD + " + GOLD);
    }
    //Attack Methods

    //AnimEvent Methods

    public void AnimEvent_Hit()
    {
        SetIdleDuration(0.5f);
        ChangeAniFromType(AnyType.IDLE);
    }
    virtual public void AnimEvent_Attack(int _areaNum)
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
    public void AnimEvent_AttackFinished()
    {
        SetIdleDuration(1.2f);
    }

    public void AnimEvent_attackSounds(int _num)
    {
        if (_num == 0)
            AudioManager.Instance.monsterPlay(AudioManager.Instance.mush_slimeAttack);
        else
            AudioManager.Instance.monsterPlay(AudioManager.Instance.GnollAttack);
    }

    public void AnimEvent_deadSounds(int _num)
    {
        if(_num == 0)
            AudioManager.Instance.monsterPlay(AudioManager.Instance.mush_slimedead);
        else
            AudioManager.Instance.monsterPlay(AudioManager.Instance.Gnoalldead);
    }

    public void AnimEvent_growling()
    {
        AudioManager.Instance.monsterPlay(AudioManager.Instance.Growling);
    }
    //AnimEvent Methods

    #endregion [Attack & Demage Methods]

    #region [Inventory Item Methods]

    public void ItemDrop()
    {
        if (CODE == 100)
            Inventory.Singleton.GetDropItem(0);
        else if(CODE == 101)
            Inventory.Singleton.GetDropItem(1);
        else if(CODE == 102)
            Inventory.Singleton.GetRandomDropItem(15, 5);

        Inventory.Singleton.GetRandomDropItem(2, 10);
        Inventory.Singleton.GetRandomDropItem(3, 10);
    }

    #endregion [Inventory Item Methods]

    #region [MonsterManager Script Methods]

    public void InitMonster(SpawnPos _genTransform)
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
        MonsterManager.Instance.RemoveMonster(this);
    }

    #endregion [MonsterManager Script Methods]
}
