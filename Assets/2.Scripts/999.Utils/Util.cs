using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DefineHelper;

public class Util
{

    #region [플레이어 & 몬스터 공격 함수]
    public static bool AttackDecision(float AttackerHit, float defenceDodge)
    {
        if (Mathf.Approximately(AttackerHit, 100.0f) || AttackerHit > 100f)
            return true;
        float total = AttackerHit + defenceDodge;// 공격자의 히트와 상대방의 방어율을 더한게 토탈임
        float hitRate = Random.Range(0.0f, total);
        if (hitRate <= AttackerHit)
        {
            return true;
        }
        return false;
    }
    public static float NormalDamage(float attackAtk, float defenceDef, float skillAtk = 0)
    {
        float attack = attackAtk + (attackAtk * skillAtk / 100.0f);
        return attack - defenceDef;
    }

    public static bool CriticalDecision(float criRate)
    {
        var result = Random.Range(0.0f, 100.0f);
        if (result <= criRate)
            return true;
        return false;
    }

    public static float CriticalDamage(float damage, float criAtk)
    {
        return damage + (damage * criAtk / 100.0f);
    }

    public static AttackType AttackProcess(PlayerController player, MonsterController mon, out float damage) // player Attack
    {
        AttackType type = AttackType.Dodge;
        damage = 0f;
        if (AttackDecision(player._stat.HITRATE + Inventory.Singleton.EQUIPSTAT.HITRATE, mon.DODGERATE))
        {
            type = AttackType.Normal;
            damage = NormalDamage(player._stat.ATTACK + Inventory.Singleton.EQUIPSTAT.ATTACK, mon.DEFENCE, player._stat.SKILLATTACK); // Skilldata를 이용하여 버프받으면 데미지 증가하기 위한 형식을 구현해야함. 일단 =0으로 막아놈.
            if (CriticalDecision(player._stat.CRIRATE + +Inventory.Singleton.EQUIPSTAT.CRIRATE))
            {
                type = AttackType.Critical;
                damage = CriticalDamage(damage, player._stat.CRIATTACK + Inventory.Singleton.EQUIPSTAT.CRIATTACK );
            }
        }
        return type;
    }

    public static AttackType AttackProcess(MonsterController mon, PlayerController player, out float damage) // monster Attack
    {
        AttackType type = AttackType.Dodge;
        damage = 0f;
        if (AttackDecision(mon.HITRATE, player._stat.DODGERATE + +Inventory.Singleton.EQUIPSTAT.DODGERATE))
        {
            type = AttackType.Normal;
            damage = NormalDamage(mon.ATTACK, player._stat.DEFENCE + Inventory.Singleton.EQUIPSTAT.DEFENCE, mon.SKILLATTACK); // Skilldata를 이용하여 버프받으면 데미지 증가하기 위한 형식을 구현해야함. 일단 =0으로 막아놈.
            if (CriticalDecision(mon.CRIRATE))
            {
                type = AttackType.Critical;
                damage = CriticalDamage(damage, mon.CRIATTACK);
            }
        }
        return type;
    }
    #endregion

    #region [공용 함수]
    public static GameObject FindChildObject(GameObject parent, string childName)
    {
        var childList = parent.GetComponentsInChildren<Transform>();
        var results = childList.Where(obj => obj.name.Equals(childName));
        if (results != null)
            return results.First().gameObject;
        return null;
    }
    #endregion [공용 함수]
}
