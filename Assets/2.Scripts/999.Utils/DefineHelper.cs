using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
#region [ĳ���Ϳ�]

    public enum AnyType
    {
        IDLE                        = 0,
        RUN,
        ATTACK,
        JUMP,
        SKILL1,         // ����
        SKILL2,         // ����
        SKILL3,         // ����
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

#endregion [ĳ���Ϳ�]
}
