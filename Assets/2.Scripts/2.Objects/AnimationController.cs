using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class AnimationController : MonoBehaviour
{
    //이거 다형성임 부모 자식간
    Animator _animController;
    Dictionary<string, float> m_dicComboInputTime = new Dictionary<string, float>();
    string m_prevMotion;

    void Awake()
    {
        _animController = GetComponent<Animator>();
        CalculateCombonputTime();
    }

    public void AnimatorResetting()
    {
        _animController = GetComponent<Animator>();
    }

    public void CalculateCombonputTime()
    {
       
        var cilps = _animController.runtimeAnimatorController.animationClips; //애니메이션안에 등록되어있는 클립들 정보
        for (int i = 0; i < cilps.Length; i++)
        {
            if (cilps[i].events.Length >= 2)
            {
                float attackTime = cilps[i].events[0].time;
                float endFrameTime = cilps[i].events[1].time;
                float result = (endFrameTime - attackTime);
                m_dicComboInputTime.Add(cilps[i].name, result);
            }
        }
    }
    public float GetComboInputTime(string animName)
    {
        float time = 0f;
        m_dicComboInputTime.TryGetValue(animName, out time);
        return time;
    }
    public void Play(string animName, bool isBlend = true)
    {
        if(!string.IsNullOrEmpty(m_prevMotion))
        {
            _animController.ResetTrigger(m_prevMotion);
            m_prevMotion = null;
        }
        if(isBlend)
        {
            _animController.SetTrigger(animName);
        }
        else
        {
            _animController.Play(animName, 0, 0f);
        }
        m_prevMotion = animName;
    }

    public void ChangeEquipWeaponMotion(float num)
    {
        _animController.SetFloat("IdleKind", num);
    }

}
