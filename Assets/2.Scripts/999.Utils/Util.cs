using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DefineHelper;

public class Util
{

    #region [�÷��̾� & ���� ���� �Լ�]
    public static bool AttackDecision(float AttackerHit, float defenceDodge)
    {
        if (Mathf.Approximately(AttackerHit, 100.0f) || AttackerHit > 100f)
            return true;
        float total = AttackerHit + defenceDodge;// �������� ��Ʈ�� ������ ������� ���Ѱ� ��Ż��
        float hitRate = Random.Range(0.0f, total);
        if (hitRate <= AttackerHit)
        {
            return true;
        }
        return false;
    }
    public static float NormalDamage(float attackAtk, float skillAtk, float defenceDef)
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

    AttackType AttackProcess(MonsterController mon, PlayerController player, out float damage)
    {
        AttackType type = AttackType.Dodge;
        damage = 0f;
     /*   if (AttackDecision(player.HITRATE, mon_monInfo_dodgeRate))
        {
            type = AttackType.Normal;
            damage = NormalDamage(player.ATTACK, skilldata_attack, mon.monInfo.defence);
            if (CriticalDecision(player.CRIRATE))
            {
                type = AttackType.Critical;
                damage = CriticalDamage(damage, player.CRIATTACK);
            }
        }*/
        return type;
    }
    #endregion

    #region [���� �Լ�]
    public static GameObject FindChildObject(GameObject parent, string childName)
    {
        var childList = parent.GetComponentsInChildren<Transform>();
        var results = childList.Where(obj => obj.name.Equals(childName));
        if (results != null)
            return results.First().gameObject;
        return null;
    }
    #endregion [���� �Լ�]
}
