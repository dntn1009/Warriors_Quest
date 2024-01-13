using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerStat
{
    [Header("BASIC Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] Transform WeaponPos; // 무기 장착 위치
    [SerializeField] GameObject equipWeapon; // 장착한 무기
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object
    [SerializeField] GameObject[] _fxHitPrefab;
    [SerializeField] GameObject[] _fxHealPrefab;
    [SerializeField] StatusController _statusbar; // PlayerStatus
    [SerializeField] StatusController _monstatusbar; // 가운데 상단 몬스터 hp 표시
    [Header("SkILL Edit Param")]
    [SerializeField] SkillData _buffSkill; //Q Key
    [SerializeField] Transform _buffPos; // Healing(Potion) + Q Buff Skill  Transform
    [SerializeField] SkillData _crossSkill; //E Key
    [SerializeField] SkillData _jumpSkill; //R Key
    [SerializeField] Transform _ExploPos;

    [Header("Talk & Quest")]
    public QuestData _quest;
    [SerializeField] GameObject _npcObj;

    //참조 변수
    //Animator _animController;
    CharacterController _charController;
    Transform _mainCamera;
    AttackAreUnitFind[] _AttackAreUnitFind;
    TrailRenderer WeaponTrail;

    

    //정보 변수
    Vector3 mv; // Player Object Vector3
    bool _isJump; // 점프
    bool _isEquip; // 장비 착용
    bool _isPressAttack; // Attack

    bool _isBuffSkill; // skill coltime
    bool _isCrossSkill; // skill coltime
    bool _isJumpSkill; // skill coltime

    bool _isHPPotion;
    bool _isMPPotion;

    int _comboIndex; // 0~3 까지 Comoblist에잇는 anytype을 실행하기 위한 int
    float _skillatt;
    List<AnyType> _comboList = new List<AnyType>() { AnyType.ATTACK1, AnyType.ATTACK2, AnyType.ATTACK3 }; // 기본공격 시 연계되는 동작 구현하기 위함.
    Queue<KeyCode> _keyBuffer = new Queue<KeyCode>();

    //GetAnimState 확인용 BOOL
    bool _isAttack
    {
        get
        {
            if (GetAnimState() == AnyType.ATTACK1 || GetAnimState() == AnyType.ATTACK2 || GetAnimState() == AnyType.ATTACK3) // 기본 공격시 되는 것들과 안되는 것들 구분하기 위함.  
                return true;
            return false;
        }
    }
    bool _isSkill
    {
        get
        {
            if (GetAnimState() == AnyType.BUFFSKILL || GetAnimState() == AnyType.CROSSSKILL || GetAnimState() == AnyType.JUMPSKILL)
                return true;
            return false;
        }
    }
    bool _isBasic
    {
        get
        {
            if (GetAnimState() == AnyType.IDLE || GetAnimState() == AnyType.RUN)
                return true;
            return false;
        }
    }
    public bool _isDeath
    {
        get
        {
            if (GetAnimState() == AnyType.DEATH)
                return true;
            return false;
        }
    }
    //

    void Awake()
    {
        AnimatorResetting();
        _mainCamera = Camera.main.transform;
        _charController = GetComponent<CharacterController>();
        _isEquip = false;
        _isJump = false;
        _isPressAttack = false;
        _isBuffSkill = false;
        _isCrossSkill = false;
        _isJumpSkill = false;
        _isHPPotion = false;
        _isMPPotion = false;
    }

    private void Start()
    {
        InitializeSet();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_npcObj != null)
            {
              IngameManager.Instance.TalkOpen();
            }
        } // NPC 대화

        if (Cursor.visible)
        {
            if (GetAnimState() != AnyType.IDLE)
                ChangeAniFromType(AnyType.IDLE);
            return;
        } // cursor 잠금시 움직이도록
        if (_isSkill)    // 스킬 발동 시
            return;

        if (IngameManager.Instance.TalkActiveSelf()) // NPC 대화 창이 열렸을 시
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isAttack && !_isJump)
            {
                _isJump = true;
                ChangeAniFromType(AnyType.JUMP);
            }
        } // Jump
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquipWeapon(!_isEquip);
        } // 무기 장착/미장착
        if (Input.GetMouseButtonDown(0))
        {
            if (_isBasic && _isEquip)
            {
                ChangeAniFromType(AnyType.ATTACK1);
                AttackForward();
                WeaponTrail.enabled = true;
            }
            if (_isAttack)
            {
                if (IsInvoking("ReleaseKeyBuffer"))
                    CancelInvoke("ReleaseKeyBuffer");
                Invoke("ReleaseKeyBuffer", GetComboInputTime(GetAnimState().ToString()));
                _keyBuffer.Enqueue(KeyCode.Mouse0);
            }

            _isPressAttack = true;
        } // 일반 공격
        if (Input.GetMouseButtonUp(0))
        {
            _isPressAttack = false;
        } // 일반 공격
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_isEquip && !_isBuffSkill && _isBasic)
            {
                _isBuffSkill = !_isBuffSkill;
                ChangeAniFromType(AnyType.BUFFSKILL);
                WeaponTrail.enabled = true;
                BuffSkill(_buffSkill, _buffPos);
                return;
            }
        } // 스킬
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isEquip && !_isCrossSkill && _isBasic)
            {
                _isCrossSkill = !_isCrossSkill;
                _fxHitPrefab[2].SetActive(true);
                ChangeAniFromType(AnyType.CROSSSKILL);
                WeaponTrail.enabled = true;
                CrossSkill(_crossSkill);
                return;
            }
        } // 스킬
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_isEquip && !_isJumpSkill && _isBasic)
            {
                _isJumpSkill = !_isJumpSkill;
                _fxHitPrefab[3].SetActive(true);
                ChangeAniFromType(AnyType.JUMPSKILL);
                WeaponTrail.enabled = true;
                JumpSkill(_jumpSkill);
                return;
            }
        } // 스킬

        //공격메서드 추가
        _charController.Move(mv * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (Cursor.visible || IngameManager.Instance.TalkActiveSelf())
        {
            mv = Vector3.zero;
            ChangeAniFromType(AnyType.IDLE);
            return;
        }

        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPCTalk")
        {
            _npcObj = other.gameObject;
            _npcObj.transform.GetChild(0).GetComponent<SetNPCUI>().TalkObjSetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "NPCTalk")
        {
            _npcObj.transform.GetChild(0).GetComponent<SetNPCUI>().TalkObjSetActive(false);
            _npcObj = null;
            IngameManager.Instance.QuestExit();
        }
    }

    #region [Character Setting Methods]
    public void InitializeSet()
    {
        //임시
        _stat = new Stat("수레야", 5, 600, 600, 300, 300, 50, 0, 100, 15, 25, 10, 60, 0, 5000);
        // Player Stat Setting 구현해야함.
        _statusbar.Init_StatusSetting(this);
        _AttackAreUnitFind = _AttackAreaPrefab.GetComponentsInChildren<AttackAreUnitFind>();
        for (int i = 0; i < _AttackAreUnitFind.Length; i++)
        {
            _AttackAreUnitFind[i].Initialize(this);
        }
    }
    #endregion [Character Setting Methods]

    #region [Character Move & Jump Methods]
    //Move Methods
    public void Move()
    {
        if (_isAttack)
        {
            mv = Vector3.zero;
            return;
        }

        if (!_isJump)
        {
            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");
            //GetAxis로 x,z 값 부여하기
            Vector3 dv = new Vector3(mx, 0, mz);
            dv = (dv.magnitude > 1) ? dv.normalized : dv; // normalized화
            mv = Player_cameraMove(dv) * _movSpeed; // 카메라 포커스에 맞춰 변경
            if (mv != Vector3.zero)
            {
                if (GetAnimState() != AnyType.RUN)
                    ChangeAniFromType(AnyType.RUN);
                transform.forward = new Vector3(mv.x, 0, mv.z);
            }
            else
            {
                if (GetAnimState() == AnyType.RUN)
                    ChangeAniFromType(AnyType.IDLE);
            }
        }
        if (_charController.isGrounded)
            mv.y = 0f;
    }
    public Vector3 Player_cameraMove(Vector3 dv)
    {
        var cameraforward = _mainCamera.forward.normalized;
        cameraforward.y = 0f;
        var cameraright = _mainCamera.right.normalized;
        cameraright.y = 0f;
        Vector3 movedir = cameraforward * dv.z + cameraright * dv.x;
        return movedir;
    } // CharacterMove;
    //Move Methods

    //Jump Methods
    public void FirstJumpOffsetPlus(float offsetY)
    {
        mv.y = offsetY;
    }
    public void MiddleJumpOffsetZero()
    {
        mv.y = 0;
    }
    public void FalseIsJump() // Animation Event
    {
        _isJump = false;
        ChangeAniFromType(AnyType.IDLE);
    }
    //Jump Methods
    #endregion [Character Move & Jump Methods]

    #region WeaponMethods
    public void EquipWeapon(bool equip = true)
    {
        /*type에 따른 프리팹 가져와 손에 쥐어주기
        switch (type)
        {
            case WeaponType.OneHandSword:
                break;
        }*/
        if (equipWeapon == null)
            return;

        if (equip)
        {
            ChangeEquipWeaponMotion(1);
            equipWeapon.SetActive(true);
        }
        else
        {
            ChangeEquipWeaponMotion(0);
            equipWeapon.SetActive(false);
        }
        _isEquip = equip;

    }

    public void EquipmentWeapon(GameObject obj, bool nullCheck)
    {
        equipWeapon = obj;
        WeaponTrail = obj.transform.GetChild(0).GetComponent<TrailRenderer>();
        if (!nullCheck)
            ChangeEquipWeaponMotion(0);
    }
    // 무기 장착 외형 및 equipWeapon 무기 쥐어주기

    public void unEquipmentWeapon(GameObject obj, bool nullCheck)
    {
        if (nullCheck)
        {
            obj.SetActive(false);
            ChangeEquipWeaponMotion(0);
            WeaponTrail = null;
            equipWeapon = null;

        }
        else
            obj.SetActive(false);
    }
    // 무기 해제 및 equipWeaopn null 및 교체 과정


    #endregion WeaponMethopds & AnimationTypeMethods

    #region [Attack & Attack Animation Methods]

    //Attack & Demage Methods

    public void AttackForward()
    {
        var cameraforward = _mainCamera.forward.normalized;
        cameraforward.y = 0f;
        transform.forward = cameraforward;
    }

    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _stat.HP -= Mathf.CeilToInt(damage);
        if (_stat.HP <= 0f)
        {
            _stat.HP = 0;
            ChangeAniFromType(AnyType.DEATH);
        }

        _statusbar.SetHP(this);

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Normal)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Player_Hit").transform.position, damage.ToString(), Color.gray); //데미지  UI 표시
        if (attackType == AttackType.Critical)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Player_Hit").transform.position, damage.ToString(), Color.yellow); //데미지  UI 표시

    }
    //Attack Methods

    //AnimEvent Methods
    public void AnimEvent_Attack(int _areaNum)
    {
        float damage = 0f;
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
                Debug.Log("데미지 : " + damage);
                _monstatusbar.MonsterSetHP(mon);
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
            _AttackAreUnitFind[i].UnitList.RemoveAll(obj => obj.GetComponent<MonsterController>()._isDeath);
    }

    public void AnimEvent_AttackFinished()
    {
        bool _isCombo = false;
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
            WeaponTrail.enabled = true;
        }
        else
        {
            ChangeAniFromType(AnyType.IDLE);
            WeaponTrail.enabled = false;
            _comboIndex = 0;
        }
    }
    //AnimEvent Methods

    #endregion [Attack & Attack Animation Methods]

    #region [Skill Methods]
    public void BuffSkill(SkillData skilldata, Transform _pos)
    {
        var obj = Instantiate(skilldata._fxSkillPrefab, _pos);
        _stat.ATTACK += skilldata._demage;
        StartCoroutine(Couroutine_BuffSkill(skilldata._demage, skilldata._coolTime));
    }

    public void CrossSkill(SkillData skilldata)
    {
        _skillatt = skilldata._demage;
        _stat.SKILLATTACK = _skillatt;
        StartCoroutine(Couroutine_CrossSkill(skilldata._coolTime));
    }

    public void JumpSkill(SkillData skilldata)
    {
        _skillatt = skilldata._demage;
        _stat.SKILLATTACK = _skillatt;
        StartCoroutine(Couroutine_JumpSkill(skilldata._coolTime));
    }

    public void AnimEvent_AttackSkill(int _areaNum)
    {
        float damage = 0f;
        var unitList = _AttackAreUnitFind[_areaNum].UnitList;

        if (GetAnimState() == AnyType.JUMPSKILL)
        {
            var obj = Instantiate(_jumpSkill._fxSkillPrefab);
            obj.transform.position = _ExploPos.transform.position;
            IngameManager.Instance.EffectParent(obj);
        }


        for (int i = 0; i < unitList.Count; i++)
        {
            var mon = unitList[i].GetComponent<MonsterController>();
            var dummy = Util.FindChildObject(unitList[i], "Monster_Hit");
            if (dummy != null && mon != null)
            {
                if (mon._isDeath) continue;
                AttackType type = Util.AttackProcess(this, mon, out damage);
                mon.SetDemage(type, damage);
                Debug.Log("스킬 데미지 : " + damage);
                _monstatusbar.MonsterSetHP(mon);
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
            _AttackAreUnitFind[i].UnitList.RemoveAll(obj => obj.GetComponent<MonsterController>()._isDeath);
    } // AttackSkill Demage 시작점

    public void AnimEvent_SkillFinished()
    {
        _skillatt = 0;
        _stat.SKILLATTACK = 0;
        WeaponTrail.enabled = false;
        if (GetAnimState() == AnyType.CROSSSKILL)
            _fxHitPrefab[2].SetActive(false);
        else if (GetAnimState() == AnyType.JUMPSKILL)
            _fxHitPrefab[3].SetActive(false);
        ChangeAniFromType(AnyType.IDLE);
    } // AttackSkill 애니메이션 종료

    #endregion

    #region Attack KeyBufferMethods
    void ReleaseKeyBuffer()
    {
        _keyBuffer.Clear();
    }
    #endregion

    #region [Quest Methods]

    public void QuestTypeKill(MonsterController mon)
    {
        if (_quest.isActive)
        {
           _quest.questGoal.EnemyKilled(mon);
        }
    }

    public void QuestComplete()
    {
            _quest.Complete(this);
            _quest = new QuestData();
    }

    #endregion [Quest Methods]

    #region [NPC & Quest Methods]

    public GameObject npcObjSet()
    {
        return _npcObj;
    }

    #endregion [NPC & Quest Methods]

    #region [PotionItem Use Methods]

    public void PotionItemEffect(GameObject fxPrefab, Transform _pos)
    {
        var obj = Instantiate(fxPrefab, _pos);
    }

    public void UsePotionItem(InventoryItem item)
    {
        if (item.myItem.hp > 0)
        {
            if (_isHPPotion || _stat.HP >= _stat.MAXHP)
                return;

            _isHPPotion = true;
            PotionItemEffect(_fxHealPrefab[0], _buffPos);
            if (_stat.HP + item.myItem.hp >= _stat.MAXHP)
                _stat.HP = _stat.MAXHP;
            else
                _stat.HP += item.myItem.hp;
            _statusbar.SetHP(this);
            StartCoroutine(Couroutine_HPPotion(5));
        }
        else if (item.myItem.mp > 0)
        {
            if (_isMPPotion || _stat.MP >= _stat.MAXMP)
                return;

            _isMPPotion = true;
            PotionItemEffect(_fxHealPrefab[1], _buffPos);
            if (_stat.MP + item.myItem.mp >= _stat.MAXMP)
                _stat.MP = _stat.MAXMP;
            else
                _stat.MP += item.myItem.mp;
            _statusbar.SetMP(this);
            StartCoroutine(Couroutine_MPPotion(5));
        }

        if (item.currentCount == 1)
        {
            Destroy(item.gameObject);
            Inventory.Singleton.SetItemInfoNull();
        }
        else
            item.decreaseCount(1);
    }

    #endregion [PotionItem Use Methods]

    #region [Couroutine Methods]
    IEnumerator Couroutine_BuffSkill(float buffatk, float cooltime)
    {
        float Initcool = cooltime;

        while (cooltime >= 1f)
        {
            cooltime -= Time.deltaTime;
            IngameManager.Instance.Qskill_CoolTime(Initcool, cooltime);
            yield return new WaitForFixedUpdate();
        }
        _stat.ATTACK -= buffatk;
        _isBuffSkill = !_isBuffSkill;
    } // BuffSkill 쿨타임

    IEnumerator Couroutine_CrossSkill(float cooltime)
    {
        float Initcool = cooltime;
        while (cooltime >= 0f)
        {
            cooltime -= Time.deltaTime;
            IngameManager.Instance.Eskill_CoolTime(Initcool, cooltime);
            yield return new WaitForFixedUpdate();
        }
        _isCrossSkill = !_isCrossSkill;
    } // CrossSkill 쿨타임
    IEnumerator Couroutine_JumpSkill(float cooltime)
    {
        float Initcool = cooltime;

        while (cooltime >= 0f)
        {
            cooltime -= Time.deltaTime;
            IngameManager.Instance.Rskill_CoolTime(Initcool, cooltime);
            yield return new WaitForFixedUpdate();
        }
        _isJumpSkill = !_isJumpSkill;
    } // JumpSkill 쿨타임
    IEnumerator Couroutine_HPPotion(float cooltime)
    {
        while (cooltime >= 0f)
        {
            cooltime -= Time.deltaTime;
            //image.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        _isHPPotion = !_isHPPotion;
    } // HPPotion 쿨타임
    IEnumerator Couroutine_MPPotion(float cooltime)
    {
        while (cooltime >= 1f)
        {
            cooltime -= Time.deltaTime;
            //image.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        _isMPPotion = !_isMPPotion;
    } // MPPotion 쿨타임

    #endregion [Couroutine Methods]

}
