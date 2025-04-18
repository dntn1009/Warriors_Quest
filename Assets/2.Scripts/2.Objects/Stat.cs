using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // ���� ���� (Data)

     //Info
    [SerializeField] string _name;
    [SerializeField] int _level;
    [SerializeField] int _maxhp;
    [SerializeField] int _hp;
    [SerializeField] int _maxmp;
    [SerializeField] int _mp;
    [SerializeField] float _attack;
    [SerializeField] float _skillattack;
    [SerializeField] float _criattack;
    [SerializeField] float _crirate;
    [SerializeField] float _defence;
    [SerializeField] float _dodgerate;
    [SerializeField] float _hitrate;
    [SerializeField] int _maxexp;
    [SerializeField] int _exp;
    [SerializeField] int _gold;

    public Stat() { }

    public Stat(PlayerData playerdata)
    {
        this._name = playerdata.stat._name;
        this._level = playerdata.stat._level;
        this._maxhp = playerdata.stat._maxhp;
        this._hp = playerdata.stat._hp;
        this._maxmp = playerdata.stat._maxmp;
        this._mp = playerdata.stat._mp;
        this._attack = playerdata.stat._attack;
        this._skillattack = 0;
        this._criattack = playerdata.stat._criattack;
        this._crirate = playerdata.stat._crirate;
        this._defence = playerdata.stat._defence;
        this._dodgerate = playerdata.stat._dodgerate;
        this._hitrate = playerdata.stat._hitrate;
        this._maxexp = playerdata.stat._maxexp;
        this._exp = playerdata.stat._exp;
        this._gold = playerdata.stat._gold;
    }
    public Stat(string name, int level, int maxhp, int hp, int maxmp, int mp, float attack, float skillattack, float criattack, float crirate, float defence, float dodgerate, float hitrate, int exp, int maxexp, int gold)
    {
        this._name = name;
        this._level = level;
        this._maxhp = maxhp;
        this._hp = hp;
        this._maxmp = maxmp;
        this._mp = mp;
        this._attack = attack;
        this._skillattack = 0;
        this._criattack = criattack;
        this._crirate = crirate;
        this._defence = defence;
        this._dodgerate = dodgerate;
        this._hitrate = hitrate;
        this._maxexp = maxexp;
        this._exp = exp;
        this._gold = gold;
    }

    #region [Property]
    public string NAME { get { return _name; } set { _name = value; } }
    public int MAXHP { get { return _maxhp; } set { _maxhp = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MAXMP { get { return _maxmp; } set { _maxmp = value; } }
    public int MP { get { return _mp; } set { _mp = value; } }
    public int LEVEL { get { return _level; } set { _level = value; } }
    public float ATTACK { get { return _attack; } set { _attack = value; } }
    public float SKILLATTACK { get { return _skillattack; } set { _skillattack = value; } }
    public float CRIATTACK { get { return _criattack; } set { _criattack = value; } }
    public float CRIRATE { get { return _crirate; } set { _crirate = value; } }
    public float DEFENCE { get { return _defence; } set { _defence = value; } }
    public float DODGERATE { get { return _dodgerate; } set { _dodgerate = value; } }
    public float HITRATE { get { return _hitrate; } set { _hitrate = value; } }
    public int MAXEXP { get { return _maxexp; } set { _maxexp = value; } }
    public int EXP { get { return _exp; } set { _exp = value; } }
    public int GOLD { get { return _gold; } set { _gold = value; } }

    #endregion [Property]

    #region [Methods]

    public void LevelUP(StatData statdata)
    {
        _exp -= _maxexp;
        this._level = statdata.level;
        this._maxhp = statdata.hpmax;
        this._maxmp = statdata.mpmax;
        this._attack = statdata.attack;
        this._criattack = statdata.criattack;
        this._crirate = statdata.crirate;
        this._defence = statdata.defence;
        this._dodgerate = statdata.dodgerate;
        this._hitrate = statdata.hitrate;
        this._maxexp = statdata.expmax;
    }

    #endregion [Methods]
}
