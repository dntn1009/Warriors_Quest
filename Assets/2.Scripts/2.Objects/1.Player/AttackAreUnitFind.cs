using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{
    PlayerController _player;
    MonsterController _monster;
    private void OnTriggerEnter(Collider other)
    {
        if(_player != null && other.CompareTag("Monster"))
        {
            var _monObj = other.gameObject;
            _player.SetAttack(_monObj);
        }

        if (_monster != null && other.CompareTag("Player"))
        {
            var _playerObj = other.gameObject;
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
