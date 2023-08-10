using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : PlayerAnimController
{
    // 정보 변수 (Data)
    
    //Info
    int _maxhp;
    int _hp;
    int _maxmp;
    int _mp;
    float _attack;
    float _criattack;
    float _crirate;
    float _defence;
    float _dodgerate;
    float _hitrate;

    //position
    Vector3 _genpos;
    // 정보 변수 (Data)

# region [Property]
    public int MAXHP { get { return _maxhp; } set { _maxhp = value; } }
    public int HP    { get { return _hp; } set { _hp = value; } }
    public int MAXMP { get { return _maxmp; } set { _maxmp = value; } }
    public int MP    { get { return _mp; } set { _mp = value; } }

    public float ATTACK { get { return _attack; } set { _attack = value; } }
    public float CRIATTACK { get { return _criattack; } set { _criattack = value; } }
    public float CRIRATE { get { return _crirate; } set { _crirate = value; } }
    public float DEFENCE { get { return _defence; } set { _defence = value; } }
    public float DODGERATE { get { return _dodgerate; } set { _dodgerate = value; } }
    public float HITRATE { get { return _hitrate; } set { _hitrate = value; } }


    #endregion [Property]

}
