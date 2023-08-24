using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{
    List<GameObject> _unitList = new List<GameObject>();
    public List<GameObject> UnitList { get { return _unitList; } }

    PlayerController _player;
    MonsterController _monster;
    private void OnTriggerEnter(Collider other)
    {
        if (_player != null && other.CompareTag("Monster"))
        {
            _unitList.Add(other.gameObject);
        }
        if (_monster != null && other.CompareTag("Player"))
        {
            _unitList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null && other.CompareTag("Monster"))
        {
            _unitList.Remove(other.gameObject);
        }
        if (_monster != null && other.CompareTag("Player"))
        {
            _unitList.Remove(other.gameObject);
        }
    }

    public void Initialize(PlayerController _owner)
    {
        _player = _owner;
    }

    public void Initialize(MonsterController _owner)
    {
        _monster = _owner;
    }

}
