using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestData[] quest;

    public void npcQuest(NPCData npc)
    {
        npc._quest = quest[npc.questIndex];
    }
}
