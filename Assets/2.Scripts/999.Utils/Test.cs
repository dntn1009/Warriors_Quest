using DefineHelper;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    SkinnedMeshRenderer _sk;
    [SerializeField] Transform[] _test;

    private void Start()
    {
        _sk = GetComponent<SkinnedMeshRenderer>();
        _test = new Transform[_sk.bones.Length];

        for(int i = 0; i < _sk.bones.Length; i++)
        {
            _test[i] = _sk.bones[i];
        }
    }
}
