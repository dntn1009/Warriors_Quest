using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [SerializeField] Transform _player;
    void Update()
    {
        Vector3 _pos = _player.position;
        _pos.y = 100f;
        this.transform.position = _pos;
    }
}
