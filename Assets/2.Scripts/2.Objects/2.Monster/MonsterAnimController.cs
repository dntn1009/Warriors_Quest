using DefineHelper;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterAnimController : AnimationController
{
    [Header("Animation Edit Param")]
    [SerializeField] float _walkSpeed = 1;
    [SerializeField] float _runSpeed = 2.5f;
    [SerializeField] bool RunTypeCheck;

    protected NavMeshAgent _navAgent;
    StringBuilder m_sb = new StringBuilder();
    AnyType _currentAnyType;

    protected override void Awake()
    {
        base.Awake();
        _navAgent = GetComponent<NavMeshAgent>();
    }
    public AnyType GetAnimState()
    {
        return _currentAnyType;
    }// 1. 이걸로 Motion값을 얻은후에
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
    } //2. 모션을 얻은걸 여기에 넣는다.

    public void ChangeAniFromType(AnyType motion, float _setFloat, bool isBlend = true)
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

        if(RunTypeCheck)
            _animController.SetFloat("RunKind", _setFloat);

        m_sb.Append(motion);
        _currentAnyType = motion;
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    } //2. 모션을 얻은걸 여기에 넣는다.

    #region [Abstract Methods]
    public abstract void AnimEvent_Hit();
    public abstract void AnimEvent_attackSounds();
    public abstract void AnimEvent_deadSounds();
    public abstract void AnimEvent_growling();
    #endregion [Abstract Methods]
}
