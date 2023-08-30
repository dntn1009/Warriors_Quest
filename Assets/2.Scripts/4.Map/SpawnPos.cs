using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [SerializeField] int _monNum;

    public int _MONNUM { get { return _monNum; } set { _monNum = value; } }
}
