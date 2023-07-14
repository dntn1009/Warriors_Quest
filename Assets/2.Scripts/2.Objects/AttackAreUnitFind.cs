using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreUnitFind : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log(this.name + " : 몬스터 명중");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log(this.name + " : 몬스터 탈출");
        }
    }
}
