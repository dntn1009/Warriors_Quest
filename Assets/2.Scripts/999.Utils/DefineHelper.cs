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
        HIT,
        SKILL1,         // 단일
        SKILL2,         // 광역
        SKILL3,         // 버프
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

    #endregion [캐릭터용]

    #region [몬스터용]
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

    #region [MAP용]

    public enum MapType
    {
        Stage1                    = 0,
        Stage2,
        Max
    }

    #endregion
}
