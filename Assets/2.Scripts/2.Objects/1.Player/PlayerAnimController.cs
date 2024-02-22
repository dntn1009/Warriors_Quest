using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using DefineHelper;

public class PlayerAnimController : AnimationController
{
    StringBuilder m_sb = new StringBuilder();
    AnyType _currentAnyType;

    public override void AnimatorResetting()
    {
        base.AnimatorResetting();
        CalculateCombonputTime();
    }
    public AnyType GetAnimState()
    {
        return _currentAnyType;
    }// 1. 이걸로 Motion값을 얻은후에
    public void ChangeAniFromType(AnyType motion, bool isBlend = true)
    {
        m_sb.Append(motion);
        _currentAnyType = motion;
        Play(m_sb.ToString(), isBlend);
        m_sb.Clear();
    } //2. 모션을 얻은걸 여기에 넣는다.


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
