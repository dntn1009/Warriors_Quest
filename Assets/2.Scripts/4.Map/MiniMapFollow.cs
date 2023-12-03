using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    GameObject _objectTofollow;

    private void Start()
    {
        _objectTofollow = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        Vector3 _pos = _objectTofollow.transform.position;
        _pos.y = 100f;
        this.transform.position = _pos;
    }
}
