using DefineHelper;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] float _movSpeed;
    [SerializeField] float _rotateSpeed = 500.0f; // ī�޶�ȸ��
    [SerializeField] float _cameraOffsetX;
    [SerializeField] float _cameraOffsetY;
    [SerializeField] float _cameraOffsetZ; // ī�޶� ���󰡱�


    //���� ����
    Animator _animController;
    CharacterController _charController;
    GameObject _mainCamera;

    //���� ����
    AnyType _currentAnyType; // animator ������
    bool _isEquip; // ��� ����
    bool _isCameraFollow = false;

    void Awake()
    {
        _animController = GetComponent<Animator>();
        _charController = GetComponent<CharacterController>();
        CameraSetting();
        _isEquip = false;
    }

    void Update()
    {
        Vector3 targetPosition = transform.position;
        PlayerMove();
        CameraRotate(targetPosition);
    }

    #region [Character Move]
    public void PlayerMove()
    {
        float mz = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Horizontal");
        //GetAxis�� x,z �� �ο��ϱ�
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
        CameraToFollow();
    }
    public void CameraRotate(Vector3 targetPosition)
    {
        //Vector3 targetPosition = transform.position;
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
    #endregion [Camera_Methods]



    public void EquipWeapon(WeaponType type)
    {
        //type�� ���� ������ ������ �տ� ����ֱ�
        _isEquip = true;
        _animController.SetBool("IsWeapon", _isEquip);
    }

    public void ChangeAniFromType(AnyType type)
    {
        _currentAnyType = type;
        _animController.SetInteger("AnyType", (int)type);
    }
}
