using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // 정보 변수 (Data)

    //Info
    [SerializeField] string _name;
    [SerializeField] int _maxhp;
    [SerializeField] int _hp;
    [SerializeField] int _maxmp;
    [SerializeField] int _mp;
    [SerializeField] int _level;
    [SerializeField] float _attack;
    [SerializeField] float _skillattack;
    [SerializeField] float _criattack;
    [SerializeField] float _crirate;
    [SerializeField] float _defence;
    [SerializeField] float _dodgerate;
    [SerializeField] float _hitrate;
    [SerializeField] int _exp;

    //position
    Vector3 _genpos;
    // 정보 변수 (Data)
    public Stat(string name, int maxhp, int hp, int maxmp, int mp, int level, float attack, float skillattack, float criattack, float crirate, float defence, float dodgerate, float hitrate, int exp)
    {
        this._name = name;
        this._maxhp = maxhp;
        this._hp = hp;
        this._maxmp = maxmp;
        this._mp = mp;
        this._level = level;
        this._attack = attack;
        this._skillattack = skillattack;
        this._criattack = criattack;
        this._crirate = crirate;
        this._defence = defence;
        this._dodgerate = dodgerate;
        this._hitrate = hitrate;
        this._exp = exp;
    }

    public Stat(MonsterData _data)
    {
        this._name = _data._name;
        this._maxhp = this._hp = _data._hp;
        this._level = _data._level;
        this._attack = _data._attack;
        this._skillattack = _data._skillattack;
        this._criattack = _data._criattack;
        this._crirate = _data._crirate;
        this._defence = _data._denfence;
        this._dodgerate = _data._dodgerate;
        this._hitrate = _data._hitrate;
        this._exp = _data._exp;

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
    public int EXP { get { return _exp; } set { _exp = value; } }

    #endregion [Property]

    #region [Methods]


    #endregion [Methods]
}
