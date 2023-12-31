using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

[System.Serializable]
public class QuestGoal
{
    public QuestType questType;

    public string progressName;

    [Header("Kill")]
    public int monsterCode;

    [Header("Gathering")]
    public int itemCode;

    [Header("Amount")]
    public int requiredAmount;
    public int currentAmount;

    public QuestGoal()
    {
        questType = QuestType.Kill;
        progressName = string.Empty;
        monsterCode = 0;
        itemCode = 0;
        requiredAmount = 0;
        currentAmount = 0;
    }

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled(MonsterController mon)
    {
        if (monsterCode == mon.CODE && questType == QuestType.Kill)
        {
            if (currentAmount < requiredAmount)
            {
                currentAmount++;
                IngameManager.Instance.QuestRefresh();
            }

        }

    }
    public void ItemCollected(InventoryItem item)
    {
        if(itemCode == item.myItem.itemCode && questType == QuestType.Gathering)
        {
            currentAmount = item.currentCount;
            IngameManager.Instance.QuestRefresh();
        }
    }
}
