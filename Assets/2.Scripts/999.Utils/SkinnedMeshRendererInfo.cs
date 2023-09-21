using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshRendererInfo : MonoBehaviour
{
    SkinnedMeshRenderer _sk;
    [SerializeField] Transform[] _bones;
    
    void Start()
    {
        _sk = GetComponent<SkinnedMeshRenderer>();
        _sk.bones = _bones;
    }
}
