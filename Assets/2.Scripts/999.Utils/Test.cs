using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class Test : MonoBehaviour
{
    Animator _animController;
    AnyType _currentAnyType; // animator 움직임
    Queue<KeyCode> _keyBuffer; // 공격시 키입력으로인한 콤보를 위해만든 변수
    bool _isPressAttack;
    int _comboIndex;
    bool _isAttack
    {
        get
        {
            if (_currentAnyType == AnyType.ATTACK)
                return true;
            return false;
        }
    } // 공격

    // Start is called before the first frame update
    void Start()
    {
        _keyBuffer = new Queue<KeyCode>();
        _animController = GetComponent<Animator>();
        _isPressAttack = false;
        _comboIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
                //_isJump = true;
                break;
            case AnyType.DEATH:
                break;
        }
        _currentAnyType = type;
        _animController.SetInteger("AnyType", (int)type);
    }
    void _AttackFinished() //Animation _Finish
    {
        bool IsCombo = false; // 연타했을경우를 위한 변수
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
            if (IsInvoking("ReleaseKeyBuffer")) // 아직 리셋이 안되었단 이야기
                CancelInvoke("ReleaseKeyBuffer");
            float time = _animController.GetCurrentAnimatorStateInfo(0).length - 1;
            Debug.Log("받아온 시간 : " + time);
            Invoke("ReleaseKeyBuffer", time);
            _keyBuffer.Enqueue(KeyCode.V);
        }// 눌린시점으로부터 일정시간동안 값이 안들어오면 이값을 비울것이란 예약
        if (_currentAnyType == AnyType.IDLE || _currentAnyType == AnyType.RUN)
        {
            ChangeAniFromType(AnyType.ATTACK);
        }
        _isPressAttack = true;
    }
    public void OnReleaseAttack()
    {
        _isPressAttack = false;
    }
    void ReleaseKeyBuffer()
    {
        _keyBuffer.Clear();
    }
}
