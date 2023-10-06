using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonsterAnimController
{
    [Header("Data Edit Param")]
    [SerializeField] MonsterData _data;

    [Header("Stat Edit Param")]
    public Stat _stat;
   
    protected void Init_StatSetting()
    {
        _stat = new Stat(_data);
    }

}
