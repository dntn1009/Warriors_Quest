using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{
    PlayerController _player;

    public void Initialize(PlayerController _owner)
    {
        _player = _owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            _player.Attack(other.gameObject);
            Debug.Log(this.name + " : ���� ����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log(this.name + " : ���� Ż��");
        }
    }
}
