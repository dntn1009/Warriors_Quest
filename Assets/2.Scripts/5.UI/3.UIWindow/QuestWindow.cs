using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DefineHelper;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] GameObject none;
    [SerializeField] GameObject value;

    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] TextMeshProUGUI _questType;
    [SerializeField] TextMeshProUGUI _progressName;
    [SerializeField] TextMeshProUGUI _amount;

    [SerializeField] PlayerController _player;

    public void QuestSetting()
    {
        if (_player._quest.isActive)
        {
            value.SetActive(true);
            none.SetActive(false);

            _title.text = _player._quest.title;
            _description.text = _player._quest.description;

            if (_player._quest.questGoal.questType == QuestType.Kill)
                _questType.text = "처치하기";
            else if (_player._quest.questGoal.questType == QuestType.Gathering)
                _questType.text = "수집하기";

            _progressName.text = _player._quest.questGoal.progressName;

            _amount.text = "( " + _player._quest.questGoal.currentAmount + " / " + _player._quest.questGoal.requiredAmount + " )";
        }
        else
        {
            none.SetActive(true);
            value.SetActive(false);
        }
    }

    public void amountSetting()
    {
        _amount.text = "( " + _player._quest.questGoal.currentAmount + " / " + _player._quest.questGoal.requiredAmount + " )";
    }
}
