using DefineHelper;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
   /* [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] float _rotateSpeed = 500.0f; // 카메라회전
    [SerializeField] float _cameraOffsetX;
    [SerializeField] float _cameraOffsetY;
    [SerializeField] float _cameraOffsetZ; // 카메라 따라가기
    [SerializeField] float _cameraFixY;
    [SerializeField] Transform WeaponPos; // 무기 장착 위치
    [SerializeField] GameObject equipWeapon; // 장착한 무기
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object

    //참조 변수
    Animator _animController;
    CharacterController _charController;
    GameObject _mainCamera;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;
    //정보 변수
    AnyType _currentAnyType; // animator 움직임
    Vector3 mv; // Player Object Vector3
    bool _isJump;
    bool _isEquip; // 장비 착용
    Queue<KeyCode> _keyBuffer; // 공격시 키입력으로인한 콤보를 위해만든 변수


    bool _isAttack
    {
        get
        {
            if (_currentAnyType == AnyType.ATTACK)
                return true;
            return false;
        }
    } // 공격

    AttackType _currentAttackType;

    void Awake()
    {
        _keyBuffer = new Queue<KeyCode>();
        _animController = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        CameraSetting();
        _isEquip = false;
        _isJump = false;
    }

    private void Start()
    {
        InitializeSet();
    }

    void Update()
    {

        if (_charController.isGrounded)
            _cameraFixY = transform.position.y; // Camera Y축 고정을 위해서

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isAttack)
                ChangeAniFromType(AnyType.JUMP);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquipWeapon(WeaponType.OneHandSword, !_isEquip);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isJump)
                ChangeAniFromType(AnyType.ATTACK);
        }

        PlayerMove();
        CameraRotate(transform.position);
    }

    #region [Character Move]
    public void PlayerMove()
    {
        if (!_isJump && !_isAttack)
        {
            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");
            //GetAxis로 x,z 값 부여하기
            Vector3 dv = new Vector3(mx, 0, mz);
            dv = (dv.magnitude > 1) ? dv.normalized : dv; // normalized화
            mv = Player_cameraMove(dv) * _movSpeed; // 카메라 포커스에 맞춰 변경

            if (mv.magnitude > 0)
            {
                ChangeAniFromType(AnyType.RUN);
                transform.forward = new Vector3(mv.x, 0, mv.z);
            }
            else
                ChangeAniFromType(AnyType.IDLE);
        }
        else if (_isAttack)
            mv = Vector3.zero;

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
        //Vector3 targetPosition 이용
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
        //type에 따른 프리팹 가져와 손에 쥐어주기
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
                AttackAnyType();
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
            //attackAUF.Initialize(this); // Player로 옮길땐 무조건 주석지워주기
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
        Debug.Log(monster.name + "공격하였습니다.");
    }

    #endregion

    #region Attack KeyBufferMethods

    public void AttackAnyType()
    {
        _keyBuffer.Enqueue(KeyCode.Mouse0);
        _animController.SetInteger("AttackCombo", (int)_currentAttackType);
    }
    void ReleaseKeyBuffer()
    {
        _keyBuffer.Clear();
    }
    #endregion*/
}
