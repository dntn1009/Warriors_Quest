using DefineHelper;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] float _rotateSpeed = 500.0f; // ī�޶�ȸ��
    [SerializeField] float _cameraOffsetX;
    [SerializeField] float _cameraOffsetY;
    [SerializeField] float _cameraOffsetZ; // ī�޶� ���󰡱�
    [SerializeField] float _cameraFixY;
    [SerializeField] Transform WeaponPos; // ���� ���� ��ġ
    [SerializeField] GameObject equipWeapon; // ������ ����
    [SerializeField] GameObject _AttackAreaPrefab; // ���� ������ �ʿ��� Collider ���� Object

    //���� ����
    Animator _animController;
    CharacterController _charController;
    GameObject _mainCamera;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;
    //���� ����
    AnyType _currentAnyType; // animator ������
    Vector3 mv; // Player Object Vector3
    bool _isJump;
    bool _isEquip; // ��� ����
    Queue<KeyCode> _keyBuffer; // ���ݽ� Ű�Է��������� �޺��� ���ظ��� ����
    bool _isPressAttack;
    int _comboIndex;
    bool _isAttack { get { if (_currentAnyType == AnyType.ATTACK) 
                    return true;
                        return false;
        } } // ����
    
    void Awake()
    {
        _keyBuffer = new Queue<KeyCode>();
        _animController = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        CameraSetting();
        _isEquip = false;
        _isJump = false;
        _isPressAttack = false;
        _comboIndex = 0;
    }

    private void Start()
    {
        InitializeSet();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquipWeapon(WeaponType.OneHandSword, !_isEquip);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnPressAttack();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            OnReleaseAttack();
        }
        PlayerMove();
        CameraRotate(transform.position);
    }

    #region [Character Move]
    public void PlayerMove()
    {
        if (_charController.isGrounded)
            _cameraFixY = transform.position.y; // Camera Y�� ������ ���ؼ�

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

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeAniFromType(AnyType.JUMP);
                return;
            }

            if (!_isAttack)
            {
                if (mv.magnitude > 0)
                {
                    ChangeAniFromType(AnyType.RUN);
                    transform.forward = new Vector3(mv.x, 0, mv.z);
                }
                else
                    ChangeAniFromType(AnyType.IDLE);
            }
        }
        _charController.Move(mv * Time.deltaTime);
    }

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
    }
    #endregion [Character Move]

    #region [Camera_Methods]
    public void CameraSetting()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraToFollow(transform.position);
    }
    public void CameraRotate(Vector3 targetPosition)
    {
        //Vector3 targetPosition �̿�
        if (Input.GetMouseButton(1))
        {
            float xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;
            //float yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;
            //_mainCamera.transform.RotateAround(stagePosition, Vector3.right, -yRotateMove);
            _mainCamera.transform.RotateAround(targetPosition, Vector3.up, xRotateMove);
            _cameraOffsetX = _mainCamera.transform.position.x - targetPosition.x;
            _cameraOffsetY = _mainCamera.transform.position.y - targetPosition.y;
            _cameraOffsetZ = _mainCamera.transform.position.z - targetPosition.z;
        }
        CameraToFollow(targetPosition);
    }
    public void CameraToFollow(Vector3 targetPosition)
    {
        Vector3 FixedPos = new Vector3(targetPosition.x + _cameraOffsetX, _cameraFixY + _cameraOffsetY, targetPosition.z + _cameraOffsetZ);
        _mainCamera.transform.position = FixedPos;
        //_mainCamera.transform.LookAt(this.transform);
    }

    public Vector3 Player_cameraMove(Vector3 dv)
    {
        var cameraforward = _mainCamera.transform.forward.normalized;
        cameraforward.y = 0f;
        var cameraright = _mainCamera.transform.right.normalized;
        cameraright.y = 0f;
        Vector3 movedir = cameraforward * dv.z + cameraright * dv.x;
        return movedir;
    }
    #endregion [Camera_Methods]

    #region WeaponMethods & AnimationTypeMethods
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
        _animController.SetBool("IsWeapon", _isEquip);
    }

    public void ChangeAniFromType(AnyType type)
    {
        switch (type)
        {
            case AnyType.IDLE:
                break;
            case AnyType.RUN:
                break;
            case AnyType.ATTACK:
                _animController.SetInteger("AttackCombo", _comboIndex);
                break;
            case AnyType.JUMP:
                _isJump = true;
                break;
            case AnyType.DEATH:
                break;
        }
        _currentAnyType = type;
        _animController.SetInteger("AnyType", (int)type);
    }
    #endregion WeaponMethopds & AnimationTypeMethods

    #region Attack & Attack Animation Methods

    public void InitializeSet()
    {
        _AttackRngs = _AttackAreaPrefab.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < _AttackRngs.Length; i++)
        {
            AttackAreUnitFind attackAUF = _AttackRngs[i].GetComponent<AttackAreUnitFind>();
            attackAUF.Initialize(this);
            _AttackRngs[i].enabled = false;
        }
    }
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

    public void SetDemage(GameObject monster)
    {
        Debug.Log(monster.name + "�����Ͽ����ϴ�.");
    }

    #endregion

    #region Attack KeyBufferMethods
    void _AttackFinished() //Animation _Finish
    {
        bool IsCombo = false; // ��Ÿ������츦 ���� ����
        if (_isPressAttack)
            IsCombo = true;
        if (_keyBuffer.Count == 1)
        {
            var key = _keyBuffer.Dequeue();
            if (key == KeyCode.Space)
                IsCombo = true;
        }
        else if (_keyBuffer.Count > 1)
        {
            ReleaseKeyBuffer();
            IsCombo = false;
        }
        if (IsCombo)
        {
            _comboIndex++;
            if (_comboIndex > 2)
                _comboIndex = 0;
            ChangeAniFromType(AnyType.ATTACK);
        }
        else
        {
            ChangeAniFromType(AnyType.IDLE);
            _comboIndex = 0;
        }
    }

    public void OnPressAttack()
    {
        if (_isAttack)
        {
            if (IsInvoking("ReleaseKeyBuffer")) // ���� ������ �ȵǾ��� �̾߱�
                CancelInvoke("ReleaseKeyBuffer");
            float time = _animController.GetCurrentAnimatorStateInfo(0).length - 1;
            Debug.Log("�޾ƿ� �ð� : " + time);
            Invoke("ReleaseKeyBuffer", time);
            _keyBuffer.Enqueue(KeyCode.V);
        }// �����������κ��� �����ð����� ���� �ȵ����� �̰��� �����̶� ����
        if (_currentAnyType == AnyType.IDLE || _currentAnyType == AnyType.RUN)
        {
            ChangeAniFromType(AnyType.ATTACK);
        }
        _isPressAttack = true;
    }

    public void AttackAnimTest()
    {
        if (_currentAnyType == AnyType.IDLE || _currentAnyType == AnyType.RUN)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeAniFromType(AnyType.ATTACK);
            }
        }

        if(_currentAnyType == AnyType.ATTACK)
            KeySettingTest();
    }

    public void KeySettingTest()
    {
        // �̺κп� Ű���۸� �̿��ؾ���.
        if (_keyBuffer.Count == 0) // ���콺 Ŭ���� ���ϸ� Idle�� ���� + Attack01 ~ 03 ���� ������ �ð��ȿ� ���ϵ��� ¥����.
        {
            _comboIndex = 0;
            ChangeAniFromType(AnyType.IDLE);
        }
        else // ��� ���콺 Ŭ���� �Ѱ� �����Ǹ� ���� �޺� ����
        {
            if (_comboIndex >= 2)
                _comboIndex = 0;
            else
                _comboIndex++;
            ChangeAniFromType(AnyType.ATTACK);
        }
    }
    public void OnReleaseAttack()
    {
        _isPressAttack = false;
    }
    void ReleaseKeyBuffer()
    {
        _keyBuffer.Clear();
    }
    #endregion
}
