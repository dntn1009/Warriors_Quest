using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerStat
{
    [Header("BASIC Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] Transform WeaponPos; // ���� ���� ��ġ
    [SerializeField] GameObject equipWeapon; // ������ ����
    [SerializeField] GameObject _AttackAreaPrefab; // ���� ������ �ʿ��� Collider ���� Object
    [SerializeField] GameObject[] _fxHitPrefab;
    [SerializeField] GameObject[] _fxHealPrefab;
    [SerializeField] StatusController _statusbar; // PlayerStatus
    [SerializeField] StatusController _monstatusbar; // ��� ��� ���� hp ǥ��
    [Header("SkILL Edit Param")]
    [SerializeField] SkillData _buffSkill; //Q Key
    [SerializeField] Transform _buffPos; // Healing(Potion) + Q Buff Skill  Transform
    [SerializeField] SkillData _crossSkill; //E Key
    [SerializeField] SkillData _jumpSkill; //R Key
    [SerializeField] Transform _ExploPos;

    [Header("Talk & Quest")]
    public QuestData _quest;
    [SerializeField] GameObject _npcObj;

    //���� ����
    //Animator _animController;
    CharacterController _charController;
    Transform _mainCamera;
    AttackAreUnitFind[] _AttackAreUnitFind;
    TrailRenderer WeaponTrail;

    

    //���� ����
    Vector3 mv; // Player Object Vector3
    bool _isJump; // ����
    bool _isEquip; // ��� ����
    bool _isPressAttack; // Attack

    bool _isBuffSkill; // skill coltime
    bool _isCrossSkill; // skill coltime
    bool _isJumpSkill; // skill coltime

    bool _isHPPotion;
    bool _isMPPotion;

    int _comboIndex; // 0~3 ���� Comoblist���մ� anytype�� �����ϱ� ���� int
    float _skillatt;
    List<AnyType> _comboList = new List<AnyType>() { AnyType.ATTACK1, AnyType.ATTACK2, AnyType.ATTACK3 }; // �⺻���� �� ����Ǵ� ���� �����ϱ� ����.
    Queue<KeyCode> _keyBuffer = new Queue<KeyCode>();

    //GetAnimState Ȯ�ο� BOOL
    bool _isAttack
    {
        get
        {
            if (GetAnimState() == AnyType.ATTACK1 || GetAnimState() == AnyType.ATTACK2 || GetAnimState() == AnyType.ATTACK3) // �⺻ ���ݽ� �Ǵ� �͵�� �ȵǴ� �͵� �����ϱ� ����.  
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
        } // NPC ��ȭ

        if (Cursor.visible)
        {
            if (GetAnimState() != AnyType.IDLE)
                ChangeAniFromType(AnyType.IDLE);
            return;
        } // cursor ��ݽ� �����̵���
        if (_isSkill)    // ��ų �ߵ� ��
            return;

        if (IngameManager.Instance.TalkActiveSelf()) // NPC ��ȭ â�� ������ ��
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
        } // ���� ����/������
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
        } // �Ϲ� ����
        if (Input.GetMouseButtonUp(0))
        {
            _isPressAttack = false;
        } // �Ϲ� ����
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
        } // ��ų
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
        } // ��ų
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
        } // ��ų

        //���ݸ޼��� �߰�
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
        //�ӽ�
        _stat = new Stat("������", 5, 600, 600, 300, 300, 50, 0, 100, 15, 25, 10, 60, 0, 5000);
        // Player Stat Setting �����ؾ���.
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
            //GetAxis�� x,z �� �ο��ϱ�
            Vector3 dv = new Vector3(mx, 0, mz);
            dv = (dv.magnitude > 1) ? dv.normalized : dv; // normalizedȭ
            mv = Player_cameraMove(dv) * _movSpeed; // ī�޶� ��Ŀ���� ���� ����
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
        /*type�� ���� ������ ������ �տ� ����ֱ�
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
    // ���� ���� ���� �� equipWeapon ���� ����ֱ�

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
    // ���� ���� �� equipWeaopn null �� ��ü ����


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
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Player_Hit").transform.position, damage.ToString(), Color.gray); //������  UI ǥ��
        if (attackType == AttackType.Critical)
            IngameManager.Instance.CreateDamage(Util.FindChildObject(this.gameObject, "Player_Hit").transform.position, damage.ToString(), Color.yellow); //������  UI ǥ��

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
                Debug.Log("������ : " + damage);
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
                Debug.Log("��ų ������ : " + damage);
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
    } // AttackSkill Demage ������

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
    } // AttackSkill �ִϸ��̼� ����

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
    } // BuffSkill ��Ÿ��

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
    } // CrossSkill ��Ÿ��
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
    } // JumpSkill ��Ÿ��
    IEnumerator Couroutine_HPPotion(float cooltime)
    {
        while (cooltime >= 0f)
        {
            cooltime -= Time.deltaTime;
            //image.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        _isHPPotion = !_isHPPotion;
    } // HPPotion ��Ÿ��
    IEnumerator Couroutine_MPPotion(float cooltime)
    {
        while (cooltime >= 1f)
        {
            cooltime -= Time.deltaTime;
            //image.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
        _isMPPotion = !_isMPPotion;
    } // MPPotion ��Ÿ��

    #endregion [Couroutine Methods]

}
