using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] Transform _objectTofollow; // Player Object
    [SerializeField] Transform _realCamera; // Main Camera
    [SerializeField] float followSpeed = 10f; // ���󰡴� �ӵ�
    [SerializeField] float sensitivity = 100f; // �ΰ���
    [SerializeField] float clampAngle = 70f; // ���ѵ� ������ ����
    [SerializeField] float minDistance; // �ּҰŸ�
    [SerializeField] float maxDistance; // �ִ�Ÿ�
    [SerializeField] float finalDistance; // ���� �Ÿ�
    [SerializeField] float smoothness = 10f;

    private float rotX;
    private float rotY;
    
    public Vector3 _dirNormalized; //���⸸ �������� ����(normalized)
    public Vector3 _finalDir;


    void Awake()
    {
        _objectTofollow = GameObject.FindWithTag("Player").transform.Find("FollowCam");
    }

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        _dirNormalized = _realCamera.localPosition.normalized;
        finalDistance = _realCamera.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _objectTofollow.position, followSpeed * Time.deltaTime);
        _finalDir = transform.TransformPoint(_dirNormalized * maxDistance);

        RaycastHit hit;

        if(Physics.Linecast(transform.position, _finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        _realCamera.localPosition = Vector3.Lerp(_realCamera.localPosition, _dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
