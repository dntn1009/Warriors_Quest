using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatWindow : MonoBehaviour
{
    public static StatWindow Singleton;

    [Header("Stat Value")]
    [SerializeField] TextMeshProUGUI _attValue;
    [SerializeField] TextMeshProUGUI _criAttValue;
    [SerializeField] TextMeshProUGUI _criRateValue;
    [SerializeField] TextMeshProUGUI _defenceValue;
    [SerializeField] TextMeshProUGUI _dodgeValue;
    [SerializeField] TextMeshProUGUI _hitRateValue;

    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;
    }


    public void SetStatInfo(Stat stat)
    {
        _attValue.text = stat.ATTACK.ToString();
        _criAttValue.text = stat.CRIATTACK.ToString();
        _criRateValue.text = stat.CRIRATE.ToString();
        _defenceValue.text = stat.DEFENCE.ToString();
        _dodgeValue.text = stat.DODGERATE.ToString();
        _hitRateValue.text = stat.HITRATE.ToString();
    }

}
