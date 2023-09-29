using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : SingletonMonobehaviour<StatusController>
{
    [Header("Status")]
    [SerializeField] TextMeshProUGUI player_name;
    [SerializeField] TextMeshProUGUI player_level;

    [Header("HP / MP Bar")]
    [SerializeField] Slider Hp_bar;
    [SerializeField] Slider Mp_bar;
    [SerializeField] TextMeshProUGUI Hp_text;
    [SerializeField] TextMeshProUGUI Mp_text;


    public void Init_StatusSetting(PlayerController _player)
    {
        player_name.text = _player._stat.NAME;
        player_level.text = "Lv." + _player._stat.LEVEL;
        Hp_text.text = _player._stat.HP + " / " + _player._stat.MAXHP;
        Mp_text.text = _player._stat.MP + " / " + _player._stat.MAXMP;
    }

    public void Init_StatusSetting(MonsterController _monster)
    {

    }
}
