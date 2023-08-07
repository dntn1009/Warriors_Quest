using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "1.Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    int _hp;
    public int HP { get { return _hp; } }
}
