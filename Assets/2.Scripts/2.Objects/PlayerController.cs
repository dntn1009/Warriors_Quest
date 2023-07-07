using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class PlayerController : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed = 5f;
    [SerializeField] float _rotateSpeed = 500.0f; // 카메라회전
    [SerializeField] float _cameraOffsetY;
    [SerializeField] float _cameraOffsetZ; // 카메라 따라가기


    //참조 변수
    Animator _animController;
    CharacterController _charController;
    GameObject _mainCamera;

    //정보 변수
    AnyType _currentAnyType; // animator 움직임
    bool _isEquip; // 장비 착용

    void Awake()
    {
        _animController = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        CameraSetting();
        _isEquip = false;
    }

    void Update()
    {
        PlayerMove();
        CameraRotate();
    }

    #region [Character Move]
    public void PlayerMove()
    {
        float mz = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Horizontal");
        //GetAxis로 x,z 값 부여하기
        Vector3 dv = new Vector3(mx, 0, mz);
        dv = (dv.magnitude > 1) ? dv.normalized : dv;
        Vector3 mv = dv * _movSpeed;
        _charController.SimpleMove(mv);
        if (mv.magnitude > 0)
        {
            ChangeAniFromType(AnyType.RUN);
            transform.forward = dv;
        }
        else
            ChangeAniFromType(AnyType.IDLE);
    }
    #endregion [Character Move]

    #region [Camera_Methods]
    public void CameraSetting()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + _cameraOffsetY, transform.position.z - _cameraOffsetZ);
        _mainCamera.transform.LookAt(this.transform);
    }
    public void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            float xRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * _rotateSpeed;
            float yRotateMove = Input.GetAxis("Mouse Y") * Time.deltaTime * _rotateSpeed;

            Vector3 stagePosition = transform.position;

            _mainCamera.transform.RotateAround(stagePosition, Vector3.right, -yRotateMove);
            _mainCamera.transform.RotateAround(stagePosition, Vector3.up, xRotateMove);

        }

       CameraToFollow();
    }
    public void CameraToFollow()
    {
        Vector3 FixedPos = new Vector3(transform.position.x, transform.position.y + _cameraOffsetY, transform.position.z - _cameraOffsetZ);
        _mainCamera.transform.position = FixedPos;
        _mainCamera.transform.LookAt(this.transform);
    }


    //camera위치에 맞춰 움직이도록
    public Vector3 Move_Camerafoward(Vector3 m_dir)
    {
       Vector3 lookForward = new Vector3(_mainCamera.transform.forward.x, 0f, _mainCamera.transform.forward.z).normalized;
       Vector3 lookRight = new Vector3(_mainCamera.transform.right.x, 0f, _mainCamera.transform.right.z).normalized;


        Vector3 movedir = lookForward * m_dir.y + lookRight * m_dir.x;

        return movedir;
    }
    #endregion [Camera_Methods]
    public void EquipWeapon(WeaponType type)
    {
        //type에 따른 프리팹 가져와 손에 쥐어주기
        _isEquip = true;
        _animController.SetBool("IsWeapon", _isEquip);
    }

    public void ChangeAniFromType(AnyType type)
    {
        _currentAnyType = type;
        _animController.SetInteger("AnyType", (int)type);
    }
}
