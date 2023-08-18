using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerStat
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] Transform WeaponPos; // ���� ���� ��ġ
    [SerializeField] GameObject equipWeapon; // ������ ����
    [SerializeField] GameObject _AttackAreaPrefab; // ���� ������ �ʿ��� Collider ���� Object

    //���� ����
    //Animator _animController;
    CharacterController _charController;
    Transform _mainCamera;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;
    //���� ����
    Vector3 mv; // Player Object Vector3
    bool _isJump; // ����
    bool _isEquip; // ��� ����

   
    List<AnyType> _comboList = new List<AnyType>() { AnyType.ATTACK1, AnyType.ATTACK2, AnyType.ATTACK3 }; // �⺻���� �� ����Ǵ� ���� �����ϱ� ����.
    Queue<KeyCode> _keyBuffer = new Queue<KeyCode>();
    bool _isAttack { get { if (GetAnimState() == AnyType.ATTACK1 || GetAnimState() == AnyType.ATTACK2 || GetAnimState() == AnyType.ATTACK3) // �⺻ ���ݽ� �Ǵ� �͵�� �ȵǴ� �͵� �����ϱ� ����.  
                            return true;
                                 return false;   } }
    public bool _isDeath { get { if (GetAnimState() == AnyType.DEATH)
                return true;
                    return false;   } }
    bool _isPressAttack;
    int _comboIndex; // 0~3 ���� Comoblist���մ� anytype�� �����ϱ� ���� int

    void Awake()
    {
        AnimatorResetting();
        _mainCamera = GameObject.FindWithTag("FollowCamera").transform;
        _charController = GetComponent<CharacterController>();
        _isEquip = false;
        _isJump = false;
        _isPressAttack = false;
    }

    private void Start()
    {
        InitializeSet();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isAttack && !_isJump)
            {
                _isJump = true;
                ChangeAniFromType(AnyType.JUMP);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquipWeapon(WeaponType.OneHandSword, !_isEquip);
        }
        if(Input.GetMouseButtonDown(0))
        {
            if ((GetAnimState() == AnyType.IDLE || GetAnimState() == AnyType.RUN) && _isEquip)
            {
                ChangeAniFromType(AnyType.ATTACK1);
                AttackForward();
            }
            if(_isAttack)
            {
                if (IsInvoking("ReleaseKeyBuffer"))
                    CancelInvoke("ReleaseKeyBuffer");
                Invoke("ReleaseKeyBuffer", GetComboInputTime(GetAnimState().ToString()));
                _keyBuffer.Enqueue(KeyCode.Mouse0);
            }

            _isPressAttack = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            _isPressAttack = false;
        }
        //���ݸ޼��� �߰�
        Move();
    }

    #region [Character Setting Methods]
    public void InitializeSet()
    {
        //�ӽ�
        _stat = new Stat(600, 600, 300, 300, 50, 0, 100, 15, 25, 10, 60);
        // Player Stat Setting �����ؾ���.

        _AttackRngs = _AttackAreaPrefab.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < _AttackRngs.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackRngs[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
            _AttackRngs[i].enabled = false;
        }
    }
    #endregion [Character Setting Methods]

    #region [Character Move & Jump Methods]
    //Move Methods
    public void Move()
    {
        if(_isAttack)
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
        _charController.Move(mv * Time.deltaTime);
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
    public void EquipWeapon(WeaponType type, bool equip = true)
    {
        //type�� ���� ������ ������ �տ� ����ֱ�
        switch (type)
        {
            case WeaponType.OneHandSword:
                if (equip)
                {
                    equipWeapon = Instantiate(IngameManager.Instance.SwordWeapons[0], WeaponPos);
                }
                else
                    Destroy(equipWeapon);
                break;
            case WeaponType.OneHandMace:
                break;
        }

        _isEquip = equip;
        if (_isEquip)
            ChangeEquipWeaponMotion(1);
        else
            ChangeEquipWeaponMotion(0);
    }

   
    #endregion WeaponMethopds & AnimationTypeMethods

    #region [Attack & Attack Animation Methods]

    //Attack & Demage Methods

    public void AttackForward()
    {
        var cameraforward = _mainCamera.forward.normalized;
        cameraforward.y = 0f;
        transform.forward = cameraforward;
    }

    public void SetAttack(GameObject monster)
    {
        var mon = monster.GetComponent<MonsterController>();
        var dummy = Util.FindChildObject(monster, "Monster_Hit");
        float demage = 0f;
        Debug.Log("1�� Ȯ��");
        if (!mon._isDeath && dummy != null)
        {
            Debug.Log("��");
            AttackType type = Util.AttackProcess(this, mon, out demage);
            mon.SetDemage(type, demage);
            if (type == AttackType.Dodge) return;
        }
    }

    public void SetDemage(AttackType attackType, float damage)
    {
        if (_isDeath) return;
        _stat.HP -= Mathf.CeilToInt(damage);
        //m_hudCtr.DisplayDamage(attackType, damage, playInfo.hp / (float)playInfo.hpMax);
        //������  UI ǥ��

        if (attackType == AttackType.Dodge) return;

        if (attackType == AttackType.Critical)
            ChangeAniFromType(AnyType.HIT, false);

        if (_stat.HP <= 0f)
        {
            ChangeAniFromType(AnyType.DEATH);
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
        bool _isCombo = false;
        if (_isPressAttack) // ������ ������ _isPressAttack�� true�̱� ������ isCombo�� true�� ��.
            _isCombo = true;
        if(_keyBuffer.Count == 1) // keybuffer�� �̿��Ͽ� Ÿ�̹� �ȿ� ������ �׾ȿ� count�� 1�̸� �� �Ŀ� _isComobo�� true ��Ŵ
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
        }
    }
    //AnimEvent Methods

    #endregion [Attack & Attack Animation Methods]

    #region Attack KeyBufferMethods
    void ReleaseKeyBuffer()
    {
        _keyBuffer.Clear();
    }
    #endregion

    #region Couroutine Methods

    #endregion
}
