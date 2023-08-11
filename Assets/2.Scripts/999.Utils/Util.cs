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
        if (AttackDecision(player._stat.HITRATE, mon._stat.DODGERATE))
        {
            type = AttackType.Normal;
            damage = NormalDamage(player._stat.ATTACK, mon._stat.DEFENCE, player._stat.BUFFATTACK); // Skilldata�� �̿��Ͽ� ���������� ������ �����ϱ� ���� ������ �����ؾ���. �ϴ� =0���� ���Ƴ�.
            if (CriticalDecision(player._stat.CRIRATE))
            {
                type = AttackType.Critical;
                damage = CriticalDamage(damage, player._stat.CRIATTACK);
            }
        }
        return type;
    }
    public static AttackType AttackProcess(MonsterController mon, PlayerController player, out float damage) // monster Attack
    {
        AttackType type = AttackType.Dodge;
        damage = 0f;
        if (AttackDecision(mon._stat.HITRATE, player._stat.DODGERATE))
        {
            type = AttackType.Normal;
            damage = NormalDamage(mon._stat.ATTACK, player._stat.DEFENCE, mon._stat.BUFFATTACK); // Skilldata�� �̿��Ͽ� ���������� ������ �����ϱ� ���� ������ �����ؾ���. �ϴ� =0���� ���Ƴ�.
            if (CriticalDecision(mon._stat.CRIRATE))
            {
                type = AttackType.Critical;
                damage = CriticalDamage(damage, mon._stat.CRIATTACK);
            }
        }
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
