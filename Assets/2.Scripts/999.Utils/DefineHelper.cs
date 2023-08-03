using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    #region [캐릭터용]

    public enum AnyType
    {
        IDLE                        = 0,
        WALK,
        RUN,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        JUMP,
        SKILL1,         // 단일
        SKILL2,         // 광역
        SKILL3,         // 버프
        DEATH                       = 99
    }

    public enum AttackType
    {
        Attack1                     = 0,
        Attack2,
        Attack3
    }

    public enum WeaponType
    {
        OneHandSword                = 0,
        OneHandMace
    }

#endregion [캐릭터용]
}
