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
        _attValue.text = _player._stat.ATTACK.ToString();
        _criAttValue.text = _player._stat.CRIATTACK.ToString();
        _criRateValue.text = _player._stat.CRIRATE.ToString();
        _defenceValue.text = _player._stat.DEFENCE.ToString();
        _dodgeValue.text = _player._stat.DODGERATE.ToString();
        _hitRateValue.text = _player._stat.HITRATE.ToString();
    }

}
