using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefineHelper;

public class MiniquestWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] PlayerController _player;

    public void miniQuestSetting()
    {
        this.gameObject.SetActive(true);
        nameText.text = _player._quest.questGoal.progressName;

        Inventory.Singleton.InitQuestCheck(_player);

       amountText.text = "( " + _player._quest.questGoal.currentAmount + " / " + _player._quest.questGoal.requiredAmount + " )";
    }

    public void completeSetting()
    {
        this.gameObject.SetActive(false);
    }

    public void currentAmountChange()
    {
        amountText.text = "( " + _player._quest.questGoal.currentAmount + " / " + _player._quest.questGoal.requiredAmount + " )";
    }
}
