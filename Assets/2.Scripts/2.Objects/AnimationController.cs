using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public abstract class AnimationController : MonoBehaviour
{
    //다형성 부모 자식간
    protected Animator _animController;
    Dictionary<string, float> m_dicComboInputTime = new Dictionary<string, float>();
    string m_prevMotion;

    protected virtual void Awake()
    {
        _animController = GetComponent<Animator>();
        CalculateCombonputTime();
    }

    #region [public Animation function Methods]
    public void Play(string animName, bool isBlend = true)
    {
        if (!string.IsNullOrEmpty(m_prevMotion))
        {
            _animController.ResetTrigger(m_prevMotion);
            m_prevMotion = null;
        }
        if (isBlend)
        {
            _animController.SetTrigger(animName);
        }
        else
        {
            _animController.Play(animName, 0, 0f);
        }
        m_prevMotion = animName;
    }
    #endregion [public Animation function Methods]

    #region [Player Animation Methods]
    public void CalculateCombonputTime()
    {
       
        var clips = _animController.runtimeAnimatorController.animationClips; //애니메이션안에 등록되어있는 클립들 정보
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].events.Length >= 2)
            {
                float attackTime = clips[i].events[0].time;
                float endFrameTime = clips[i].events[1].time;
                float result = (endFrameTime - attackTime);
                m_dicComboInputTime.Add(clips[i].name, result);
            }
        }
    }
    public float GetComboInputTime(string animName)
    {
        float time = 0f;
        m_dicComboInputTime.TryGetValue(animName, out time);
        return time;
    }
    public void ChangeEquipWeaponMotion(float num)
    {
        _animController.SetFloat("IdleKind", num);
    }
    #endregion [Player Animation Methods]

    #region [Animation Methods]
    public abstract void AnimEvent_Attack(int _areNum);
    public abstract void AnimEvent_AttackFinished();
    #endregion [Animation Methods]

}
