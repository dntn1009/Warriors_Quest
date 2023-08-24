using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineHelper
{
    #region [ĳ���Ϳ�]

    public enum AnyType
    {
        IDLE                        = 0,
        WALK,
        RUN,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        JUMP,
        HIT,
        SKILL1,         // ����
        SKILL2,         // ����
        SKILL3,         // ����
        DEATH                       = 99
    }

    public enum AttackType
    {
        Normal,
        Critical,
        Dodge,
        Max
    }

    public enum WeaponType
    {
        OneHandSword                = 0,
        OneHandMace
    }

    #endregion [ĳ���Ϳ�]

    #region [���Ϳ�]
    public enum BehaviourState
    {
        IDLE                        = 0,
        CHASE,
        PATROL,
        ATTACK1,
        ATTACK2,
        DEMAGED,
        DEATH,
        Max
    }

    #endregion

    #region [MAP��]

    public enum MapType
    {
        Stage1                    = 0,
        Stage2,
        Max
    }

    #endregion
}
