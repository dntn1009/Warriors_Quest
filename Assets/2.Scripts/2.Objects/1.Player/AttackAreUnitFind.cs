using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{
    PlayerController _player;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            var _monObj = other.gameObject;
            _player.SetDemage(_monObj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster"))
        {

        }
    }

    public void Initialize(PlayerController _owner)
    {
        _player = _owner;
    }

}
