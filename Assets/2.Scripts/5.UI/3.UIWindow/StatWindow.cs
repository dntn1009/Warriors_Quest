using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatWindow : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController _player;
 
    [Header("Stat Value")]
    [SerializeField] TextMeshProUGUI _attValue;
    [SerializeField] TextMeshProUGUI _criAttValue;
    [SerializeField] TextMeshProUGUI _criRateValue;
    [SerializeField] TextMeshProUGUI _defenceValue;
    [SerializeField] TextMeshProUGUI _dodgeValue;
    [SerializeField] TextMeshProUGUI _hitRateValue;

    public void SetStatInfo()
    {
        _attValue.text = _player._stat.ATTACK.ToString() + +Inventory.Singleton.EQUIPSTAT.ATTACK;
        _criAttValue.text = _player._stat.CRIATTACK.ToString() + Inventory.Singleton.EQUIPSTAT.CRIATTACK;
        _criRateValue.text = _player._stat.CRIRATE.ToString() + Inventory.Singleton.EQUIPSTAT.CRIRATE;
        _defenceValue.text = _player._stat.DEFENCE.ToString() + Inventory.Singleton.EQUIPSTAT.DEFENCE;
        _dodgeValue.text = _player._stat.DODGERATE.ToString() + Inventory.Singleton.EQUIPSTAT.DODGERATE;
        _hitRateValue.text = _player._stat.HITRATE.ToString() + Inventory.Singleton.EQUIPSTAT.HITRATE;
    }

}
