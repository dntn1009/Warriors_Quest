using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DefineHelper;
using UnityEngine.AI;

public class MonsterAnimController : AnimationController
{
    [Header("Animation Edit Param")]
    [SerializeField] float _walkSpeed = 1;
    [SerializeField] float _runSpeed = 2.5f;

    protected NavMeshAgent _navAgent;
    StringBuilder m_sb = new StringBuilder();
    AnyType _currentAnyType;

    public override void AnimatorResetting()
    {
        base.AnimatorResetting();
        _navAgent = GetComponent<NavMeshAgent>();
    }
    public AnyType GetAnimState()
    {
        return _currentAnyType;
    }// 1. �̰ɷ� Motion���� �����Ŀ�
    public void ChangeAniFromType(AnyType motion, bool isBlend = true)
    {
        switch (motion)
        {
            case AnyType.WALK:
                motion = AnyType.RUN;
                _navAgent.speed = _walkSpeed;
                break;
            case AnyType.RUN:
                _navAgent.speed = _runSpeed;
                break;
        }
        m_sb.Append(motion);
        _currentAnyType = motion;
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    } //2. ����� ������ ���⿡ �ִ´�.
}
