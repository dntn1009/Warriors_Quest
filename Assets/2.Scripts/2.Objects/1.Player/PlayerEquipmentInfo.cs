using DefineHelper;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentInfo : MonoBehaviour
{
    [Header("Body & Underwear")]
    [SerializeField] GameObject Hair;
    [SerializeField] GameObject Chest;
    [SerializeField] GameObject Arms;
    [SerializeField] GameObject Hands;
    [SerializeField] GameObject Legs;
    [SerializeField] GameObject Feet;
    [SerializeField] GameObject Underwear;

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

    [Header("Weapon")]
    [SerializeField] GameObject Wooden_Stick;
    [SerializeField] GameObject Steel_Sword;
    [SerializeField] GameObject Steel_Master_Sword;

    Dictionary<int, GameObject> EquipMentDic;

    PlayerController _player;
    private void Start()
    {
        _player = this.GetComponent<PlayerController>();

        EquipMentDic = new Dictionary<int, GameObject>();
        EquipMentDic.Add(102, Starter_Chest);
        EquipMentDic.Add(103, Starter_Pants);
        EquipMentDic.Add(104, Starter_Boots);
        EquipMentDic.Add(201, SteelAmor_Helmet);
        EquipMentDic.Add(202, SteelAmor_Chest);
        EquipMentDic.Add(203, SteelAmor_Pants);
        EquipMentDic.Add(204, SteelAmor_Boots);
        EquipMentDic.Add(205, SteelAmor_Gloves);
        EquipMentDic.Add(206, SteelAmor_Shoulders);
        EquipMentDic.Add(100, Wooden_Stick);
        EquipMentDic.Add(200, Steel_Sword);
        EquipMentDic.Add(300, Steel_Master_Sword);
    }

    #region [Amors Methods]

    public void equipEquipment(InventoryItem item, bool nullCheck)
    {
        var obj = EquipMentDic[item.myItem.equipCode];
        if (item.myItem.itemTag == SlotTag.Weapon)
            _player.EquipmentWeapon(obj, nullCheck);
        else
        {
            obj.SetActive(true);
            if (nullCheck)
                BodySetActive(item.myItem, !nullCheck);
        }

    }
    //장비 및 무기 장착 과정

    public void unequipEquipment(InventoryItem item, bool nullCheck)
    {
        var obj = EquipMentDic[item.myItem.equipCode];
        if (item.myItem.itemTag == SlotTag.Weapon)
            _player.unEquipmentWeapon(obj, nullCheck);
        else
        {
            obj.SetActive(false);
            if (nullCheck)
                BodySetActive(item.myItem, nullCheck);
        }
    }
    //장비 및 무기 해제 과정

    #endregion [Amors Methods]

    #region [Body & Underwear SetActive Methods]
    void BodySetActive(Item item, bool set)
    {
        switch (item.itemTag)
        {
            case SlotTag.Head:
                Hair.SetActive(set);
                break;
            case SlotTag.Chest:
                Chest.SetActive(set);
                Arms.SetActive(set);
                break;
            case SlotTag.Legs:
                Underwear.SetActive(set);
                Legs.SetActive(set);
                break;
            case SlotTag.Feet:
                Feet.SetActive(set);
                break;
            case SlotTag.Gloves:
                Hands.SetActive(set);
                break;
        }
    }
    #endregion [Body & Underwear SetActive Methods]

}
