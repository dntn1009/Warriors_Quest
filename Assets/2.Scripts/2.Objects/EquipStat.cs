using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipStat
{
    //정보 변수(Data)

    [SerializeField] float _attack;
    [SerializeField] float _criattack;
    [SerializeField] float _crirate;
    [SerializeField] float _defence;
    [SerializeField] float _dodgerate;
    [SerializeField] float _hitrate;

    string[] stattextinfo;

    public EquipStat() { stattextinfo = new string[6] { "ATTACK", "CRIATT", "CRIRATE", "DEFENCE", "DODGE", "HITRATE" }; }
    public EquipStat(float attack, float criattack, float crirate, float defence, float dodgerate, float hitrate)
    {
        _attack = attack;
        _criattack = criattack;
        _crirate = crirate;
        _defence = defence;
        _dodgerate = dodgerate;
        _hitrate = hitrate;
        stattextinfo = new string[6] { "ATTACK", "CRIATT", "CRIRATE", "DEFENCE", "DODGE", "HITRATE" };
    }

    public IEnumerator GetEnumerator()
    {
        yield return ATTACK;
        yield return CRIATTACK;
        yield return CRIRATE;
        yield return DEFENCE;
        yield return DODGERATE;
        yield return HITRATE;
    }

    public string statTextInfo(int num)
    {
        return stattextinfo[num];
    }

    #region [Operator OverLoading Methods]
    public static EquipStat operator +(EquipStat _stat1, EquipStat _stat2)
    {
        return new EquipStat(_stat1.ATTACK + _stat2.ATTACK, _stat1.CRIATTACK + _stat2.CRIATTACK, _stat1.CRIRATE + _stat2.CRIRATE,
                                _stat1.DEFENCE + _stat2.DEFENCE, _stat1.DODGERATE + _stat2.DODGERATE, _stat1.HITRATE + _stat2.HITRATE);
    }

    public static EquipStat operator -(EquipStat _stat1, EquipStat _stat2)
    {
        return new EquipStat(_stat1.ATTACK - _stat2.ATTACK, _stat1.CRIATTACK - _stat2.CRIATTACK, _stat1.CRIRATE - _stat2.CRIRATE,
                                _stat1.DEFENCE - _stat2.DEFENCE, _stat1.DODGERATE - _stat2.DODGERATE, _stat1.HITRATE - _stat2.HITRATE);
    }

    #endregion [Operator OverLoading Methods]

    #region [Property]
    public float ATTACK { get { return _attack; } set { _attack = value; } }
    public float CRIATTACK { get { return _criattack; } set { _criattack = value; } }
    public float CRIRATE { get { return _crirate; } set { _crirate = value; } }
    public float DEFENCE { get { return _defence; } set { _defence = value; } }
    public float DODGERATE { get { return _dodgerate; } set { _dodgerate = value; } }
    public float HITRATE { get { return _hitrate; } set { _hitrate = value; } }
    #endregion [Property]
}
