using DefineHelper;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] float _rotateSpeed = 500.0f; // 카메라회전
    [SerializeField] float _cameraOffsetX;
    [SerializeField] float _cameraOffsetY;
    [SerializeField] float _cameraOffsetZ; // 카메라 따라가기
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
    bool _isEquip; // 장비 착용
    bool _isAttack; // 공격
    bool _isCameraFollow = false;

    void Awake()
    {
        _animController = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        CameraSetting();
        _isEquip = false;
        _isAttack = false;
    }

    private void Start()
    {
        InitializeSet();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isAttack = !_isAttack;
            ChangeAniFromType(AnyType.ATTACK);
        }
        if (_isAttack)
        {
            return;
        }

        Vector3 targetPosition = transform.position;
        PlayerMove();
        CameraRotate(targetPosition);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            EquipWeapon(WeaponType.OneHandSword, !_isEquip);
        }
    }

    #region [Character Move]
    public void PlayerMove()
    {
            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");
            //GetAxis로 x,z 값 부여하기
            Vector3 dv = new Vector3(mx, 0, mz);
            dv = (dv.magnitude > 1) ? dv.normalized : dv;
            Vector3 mv = Player_cameraMove(dv) * _movSpeed;
            _charController.SimpleMove(mv);
            if (mv.magnitude > 0)
            {
                ChangeAniFromType(AnyType.RUN);
                transform.forward = mv;
            }
            else
                ChangeAniFromType(AnyType.IDLE);
    }
    #endregion [Character Move]

    #region [Camera_Methods]
    public void CameraSetting()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraToFollow();
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
        CameraToFollow();
    }
    public void CameraToFollow()
    {
        Vector3 FixedPos = new Vector3(transform.position.x + _cameraOffsetX, transform.position.y + _cameraOffsetY, transform.position.z + _cameraOffsetZ);
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
        for(int i = 0; i < _AttackRngs.Length; i++)
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

    public void Attack(GameObject monster)
    {
        Debug.Log(monster.name + "공격하였습니다.");
    }

    #endregion
}
