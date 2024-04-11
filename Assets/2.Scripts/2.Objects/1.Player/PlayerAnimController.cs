using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using DefineHelper;

public abstract class PlayerAnimController : AnimationController
{
    StringBuilder m_sb = new StringBuilder();
    AnyType _currentAnyType;

    public AnyType GetAnimState()
    {
        return _currentAnyType;
    }// Motion값을 확인하여 판단하기 위한 메서드
    public void ChangeAniFromType(AnyType motion, bool isBlend = true)
    {
        m_sb.Append(motion);
        _currentAnyType = motion;
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    } // 모션을 바꿔주기 위한 메서드


    public void playerSfx()
    {
        switch (_currentAnyType)
        {
            case AnyType.ATTACK1:
                AudioManager.Instance.SfxPlay(AudioManager.Instance.Attack1);
                break;
            case AnyType.ATTACK2:
                AudioManager.Instance.SfxPlay(AudioManager.Instance.Attack2);
                break;
            case AnyType.ATTACK3:
                AudioManager.Instance.SfxPlay(AudioManager.Instance.Attack1);
                break;
        }
    }

}
