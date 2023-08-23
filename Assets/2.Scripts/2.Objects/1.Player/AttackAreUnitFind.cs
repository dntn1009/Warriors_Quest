using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{
    List<GameObject> m_unitList = new List<GameObject>();
    public List<GameObject> UnitList { get { return m_unitList; } }

    PlayerController _player;
    MonsterController _monster;
    private void OnTriggerEnter(Collider other)
    {
        if (_player != null && other.CompareTag("Monster"))
        {
            m_unitList.Add(other.gameObject);
        }
        if (_monster != null && other.CompareTag("Player"))
        {
            m_unitList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player != null && other.CompareTag("Monster"))
        {
            m_unitList.Remove(other.gameObject);
        }
        if (_monster != null && other.CompareTag("Player"))
        {
            m_unitList.Remove(other.gameObject);
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
