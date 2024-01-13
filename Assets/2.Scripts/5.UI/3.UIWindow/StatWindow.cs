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
        _attValue.text = (_player._stat.ATTACK + Inventory.Singleton.EQUIPSTAT.ATTACK).ToString();
        _criAttValue.text = (_player._stat.CRIATTACK + Inventory.Singleton.EQUIPSTAT.CRIATTACK).ToString();
        _criRateValue.text = (_player._stat.CRIRATE + Inventory.Singleton.EQUIPSTAT.CRIRATE).ToString();
        _defenceValue.text = (_player._stat.DEFENCE + Inventory.Singleton.EQUIPSTAT.DEFENCE).ToString();
        _dodgeValue.text = (_player._stat.DODGERATE + Inventory.Singleton.EQUIPSTAT.DODGERATE).ToString();
        _hitRateValue.text = (_player._stat.HITRATE + Inventory.Singleton.EQUIPSTAT.HITRATE).ToString();
    }

}
