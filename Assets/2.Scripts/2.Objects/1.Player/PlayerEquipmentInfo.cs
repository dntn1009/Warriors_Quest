using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentInfo : MonoBehaviour
{
    [Header("Underwear")]
    [SerializeField] GameObject underwear;

    [Header("Starter")]
    [SerializeField] GameObject Starter_Chest;
    [SerializeField] GameObject Starter_Pants;
    [SerializeField] GameObject Starter_Boots;

    [Header("SteelAmor")]
    [SerializeField] GameObject SteelAmor_Helmet;
    [SerializeField] GameObject SteelAmor_Chest;
    [SerializeField] GameObject SteelAmor_Pants;
    [SerializeField] GameObject SteelAmor_Boots;
    [SerializeField] GameObject SteelAmor_Gloves;
    [SerializeField] GameObject SteelAmor_Shoulders;


    public Dictionary<string, GameObject> _amorObjectDic;

    private void Start()
    {
        _amorObjectDic = new Dictionary<string, GameObject>();
        _amorObjectDic.Add("Starter_Chest", Starter_Chest);
        _amorObjectDic.Add("Starter_Pants", Starter_Pants);
        _amorObjectDic.Add("Starter_Boots", Starter_Boots);
        _amorObjectDic.Add("SteelAmor_Helmet", SteelAmor_Helmet);
        _amorObjectDic.Add("SteelAmor_Chest", SteelAmor_Chest);
        _amorObjectDic.Add("SteelAmor_Pants", SteelAmor_Pants);
        _amorObjectDic.Add("SteelAmor_Boots", SteelAmor_Boots);
        _amorObjectDic.Add("SteelAmor_Gloves", SteelAmor_Gloves);
        _amorObjectDic.Add("SteelAmor_Shoulders", SteelAmor_Shoulders);
    }

    public void UnderwearSetActive(bool set)
    {
        underwear.SetActive(set);
    }
}