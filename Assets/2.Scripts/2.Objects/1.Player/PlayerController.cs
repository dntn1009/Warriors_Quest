using DefineHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerStat
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] Transform WeaponPos; // 무기 장착 위치
    [SerializeField] GameObject equipWeapon; // 장착한 무기
    [SerializeField] GameObject _AttackAreaPrefab; // 공격 판정시 필요한 Collider 집합 Object

    //참조 변수
    //Animator _animController;
    CharacterController _charController;
    Transform _mainCamera;
    BoxCollider[] _AttackRngs; // AttackAreUnitFinds;
    //정보 변수
    //AnyType _currentAnyType; // animator 움직임
    Vector3 mv; // Player Object Vector3
    bool _isJump; // 점프
    bool _isEquip; // 장비 착용

    bool _isAttack;
    bool _isPressAttack;

    void Awake()
    {
        //_animController = GetComponent<Animator>();
        AnimatorResetting();
        _mainCamera = GameObject.FindWithTag("FollowCamera").transform;
        _charController = GetComponent<CharacterController>();
        _isEquip = false;
        _isJump = false;
        _isAttack = false;
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
            if (!_isAttack)
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
                ChangeAniFromType(AnyType.ATTACK1);
            _isPressAttack = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            ChangeAniFromType(AnyType.IDLE);
            _isPressAttack = false;
        }
        //공격메서드 추가
        Move();
    }

    #region [Character Move]
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
        ChangeAniFromType(AnyType.IDLE);
    }
    #endregion [Character Move]

    #region [Camera_Methods]

    public Vector3 Player_cameraMove(Vector3 dv)
    {
        var cameraforward = _mainCamera.forward.normalized;
        cameraforward.y = 0f;
        var cameraright = _mainCamera.right.normalized;
        cameraright.y = 0f;
        Vector3 movedir = cameraforward * dv.z + cameraright * dv.x;
        return movedir;
    }
    #endregion [Camera_Methods]

    #region WeaponMethods
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
        if (_isEquip)
            ChangeEquipWeaponMotion(1);
        else
            ChangeEquipWeaponMotion(0);
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
        Debug.Log(monster.name + "공격하였습니다.");
    }

    #endregion

    #region Attack KeyBufferMethods
    public void AttackComobo(bool isAttack)
    {
        /*if(isAttack && (_animController.GetCurrentAnimatorStateInfo(0).IsName("WIdle") || _animController.GetCurrentAnimatorStateInfo(0).IsName("WRun")))
        {
            var cameraforward = _mainCamera.forward.normalized;
            cameraforward.y = 0f;
            transform.forward = cameraforward;
        }// 공격하는 동안 해당 방향만 가리키게
        _animController.SetBool("IsCombo", isAttack);
        _isAttack = isAttack;*/
    }
    #endregion

    #region Couroutine Methods
    IEnumerator EquipAnimMove()
    {

        yield return null;
    }
    #endregion
}
