using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmorBonesInfo : MonoBehaviour
{
    [SerializeField] public Transform _rootBone;

    [Header("Starter")]
    [SerializeField] Transform[] Starter_Chest;
    [SerializeField] Transform[] Starter_Pants;
    [SerializeField] Transform[] Starter_Boots;


    public Dictionary<string, Transform[]> _amorBonesDic;

    void Start()
    {
        _amorBonesDic = new Dictionary<string, Transform[]>();
        _amorBonesDic.Add("Starter_Chest", Starter_Chest);
        _amorBonesDic.Add("Starter_Pants", Starter_Pants);
        _amorBonesDic.Add("Starter_Boots", Starter_Boots);
        Debug.Log(Starter_Chest[0]);
    }
}
