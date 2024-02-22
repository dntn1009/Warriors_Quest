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
        BUFFSKILL,         // ����
        CROSSSKILL,         // ũ�ν�
        JUMPSKILL,         // ����
        OVERDRIVE,
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

    public enum SlotTag 
    {
        None                        = 0,
        Potion, 
        Head,
        Chest,
        Legs,
        Feet,
        Gloves,
        Shoulders,
        Weapon 
    }

    public enum QuestType
    {
        Kill                        = 0,
        Gathering
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
        HIT,
        OVERDRIVE,
        Max
    }

    #endregion

    #region [SCENE & MAP��]

    public enum SceneType
    {
        LobbyScene      = 0,
        IngameScene
    }

    public enum DataType
    {
        StatData         = 0,
        TalkData,

    }

    public enum MapType
    {
        Stage1                    = 0,
        Stage2,
        Max
    }

    #endregion

}
