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
    [SerializeField] float _buffattack;
    [SerializeField] float _criattack;
    [SerializeField] float _crirate;
    [SerializeField] float _defence;
    [SerializeField] float _dodgerate;
    [SerializeField] float _hitrate;

    //position
    Vector3 _genpos;
    // 정보 변수 (Data)
    public Stat(string name, int maxhp, int hp, int maxmp, int mp, int level, float attack, float buffattack, float criattack, float crirate, float defence, float dodgerate, float hitrate)
    {
        this._name = name;
        this._maxhp = maxhp;
        this._hp = hp;
        this._maxmp = maxmp;
        this._mp = mp;
        this._level = level;
        this._attack = attack;
        this._buffattack = buffattack;
        this._criattack = criattack;
        this._crirate = crirate;
        this._defence = defence;
        this._dodgerate = dodgerate;
        this._hitrate = hitrate;
    }
    #region [Property]
    public string NAME { get { return _name; } set { _name = value; } }
    public int MAXHP { get { return _maxhp; } set { _maxhp = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MAXMP { get { return _maxmp; } set { _maxmp = value; } }
    public int MP { get { return _mp; } set { _mp = value; } }
    public int LEVEL { get { return _level; } set { _level = value; } }
    public float ATTACK { get { return _attack; } set { _attack = value; } }
    public float BUFFATTACK { get { return _buffattack; } set { _buffattack = value; } }
    public float CRIATTACK { get { return _criattack; } set { _criattack = value; } }
    public float CRIRATE { get { return _crirate; } set { _crirate = value; } }
    public float DEFENCE { get { return _defence; } set { _defence = value; } }
    public float DODGERATE { get { return _dodgerate; } set { _dodgerate = value; } }
    public float HITRATE { get { return _hitrate; } set { _hitrate = value; } }


    #endregion [Property]
}
