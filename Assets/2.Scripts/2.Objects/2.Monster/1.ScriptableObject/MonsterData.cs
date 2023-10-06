using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "1.Scriptable Object/Data/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    public string _name;
    public int _hp;
    public int _level;
    public float _attack;
    public float _buffattack;
    public float _criattack;
    public float _crirate;
    public float _denfence;
    public float _dodgerate;
    public float _hitrate;
    public int _exp;
}
