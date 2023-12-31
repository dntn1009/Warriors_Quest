using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] TextMeshProUGUI obj_name;
    [SerializeField] TextMeshProUGUI obj_level;

    [Header("HP / MP Bar")]
    [SerializeField] Slider Hp_bar;
    [SerializeField] Slider Mp_bar;
    [SerializeField] TextMeshProUGUI Hp_text;
    [SerializeField] TextMeshProUGUI Mp_text;

    [Header("EXP Bar")]
    [SerializeField] Slider Exp_bar;
    [SerializeField] TextMeshProUGUI Exp_text;

    #region [Init Setting Methods]
    public void Init_StatusSetting(PlayerController _player)
    {
        obj_name.text = _player._stat.NAME;
        obj_level.text = "Lv." + _player._stat.LEVEL;
        Hp_text.text = _player._stat.HP + " / " + _player._stat.MAXHP;
        Mp_text.text = _player._stat.MP + " / " + _player._stat.MAXMP;
        //Exp_text.text = _player._stat.EXP + " / " + _player._stat.MAXEXP;
    }

    public void Init_StatusSetting(MonsterStat _mon)
    {
        this.gameObject.SetActive(true);
        obj_name.text = _mon.NAME;
        obj_level.text = "Lv." + _mon.LEVEL;
        Hp_text.text = _mon.HP + " / " + _mon.HPMAX;
        float normalizedHP = _mon.HP / (float)_mon.HPMAX;
        Hp_bar.value = normalizedHP;
        if (normalizedHP <= 0f)
            Hp_bar.value = 0f;
    }
    #endregion [Init Setting Methods]

    #region [Set Bar Methods]
    public void SetHP(PlayerController _player)
    {
        float normalizedHP = _player._stat.HP / (float)_player._stat.MAXHP;
        Hp_bar.value = normalizedHP;
        if (normalizedHP <= 0f)
            Hp_bar.value = 0f;
        Hp_text.text = _player._stat.HP + " / " + _player._stat.MAXHP;
        //HPBAR VALUE 변경
    }

    public void SetMP(PlayerController _player)
    {
        float normalizedMP = _player._stat.MP / (float)_player._stat.MAXMP;
        Hp_bar.value = normalizedMP;
        if (normalizedMP <= 0f)
            Mp_bar.value = 0f;
        Mp_text.text = _player._stat.MP + " / " + _player._stat.MAXMP;
        //HPBAR VALUE 변경
    }

    public void MonsterSetHP(MonsterController mon)
    {
        Init_StatusSetting(mon);
        if (IsInvoking("SetActiveFalse"))
            CancelInvoke("SetActiveFalse");
                    Invoke("SetActiveFalse", 2f);
    }
    
    void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    #endregion [Set Bar Methods]
}
