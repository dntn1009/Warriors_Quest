using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

[System.Serializable]
public class QuestData
{
    public bool isActive;

    public string title;
    public string description;

    public Item[] item;
    public int experienceReward;
    public int goldReward;

    public QuestGoal questGoal;

    public QuestData()
    {
        isActive = false;
        title = string.Empty;
        description = string.Empty;
        item = null;
        experienceReward = 0;
        goldReward = 0;
        questGoal = new QuestGoal();
    }

    public void Complete(PlayerController player)
    {
        isActive = false;
        questGoal.currentAmount = 0;

        if(questGoal.questType == QuestType.Gathering)
            Inventory.Singleton.QuestDeliver(this);

        player._stat.GOLD += goldReward;
        player._stat.EXP += experienceReward;

        if (item != null)
        {
            for(int i = 0; i < item.Length; i++)
            {
                if(item[i].itemTag == SlotTag.Potion || item[i].itemTag == SlotTag.None)
                    Inventory.Singleton.GetRewardItem(item[i], 50);
                else
                    Inventory.Singleton.GetRewardItem(item[i]);
            }
        }

        IngameManager.Instance.SetGetInfoText("Exp + " + experienceReward);
        IngameManager.Instance.SetGetInfoText("GOLD + " + goldReward);

        IngameManager.Instance.miniQuestComplete();
    }

    public bool CompleteCheck()
    {
        return questGoal.IsReached();
    }
}
