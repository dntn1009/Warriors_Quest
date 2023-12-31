using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonsterAnimController
{
    [Header("Data Edit Param")]
    [SerializeField] MonsterData _data;

    [Header("StatHP Edit Param")]
    [SerializeField] int _hpmax;
    [SerializeField] int _hp;

    #region [Data Property]

    public int CODE { get { return _data._monCode; } }
    public string NAME { get { return _data._name; } }
    public int HPMAX { get { return _hpmax; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int LEVEL { get { return _data._level; } }
    public float ATTACK { get { return _data._attack; } }
    public float SKILLATTACK { get { return _data._skillattack; } }
    public float CRIATTACK { get { return _data._criattack; } }
    public float CRIRATE { get { return _data._crirate; } }
    public float DEFENCE { get { return _data._denfence; } }
    public float DODGERATE { get { return _data._dodgerate; } }
    public float HITRATE { get { return _data._hitrate; } }
    public int EXP { get { return _data._exp; } }
    public int GOLD { get { return _data._gold; } }
    #endregion [Data Property]

    protected void Init_HPSetting()
    {
        _hpmax = _hp = _data._hp;
    }

}
