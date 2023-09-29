using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;
    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;
    [SerializeField] InventoryInfo inventoryInfo;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Btn")]
    [SerializeField] Button _closeBtn;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    [Header("PlayerEquipmentInfo")]
    [SerializeField] PlayerEquipmentInfo _playerEquipemntInfo;
    string[] _amorPart;

    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener( delegate { SpawnInventoryItem(); } );
        _closeBtn.onClick.AddListener(delegate { Close_Inventory(); });
        _amorPart = new string[(int)SlotTag.Weapon];
    }

    void Update()
    {
        if(carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item) //아이템 옮기기
    {
        if(carriedItem != null)
        {
            if(item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        if(item.activeSlot.myTag != SlotTag.None)
        { EquipEquipment(item.activeSlot.myTag, null); }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void SetItemInfo(InventoryItem item)
    {
        if (carriedItem != null)
            return;

        if (item.myItem.itemTag == SlotTag.None || item.myItem.itemTag == SlotTag.Potion)
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, string.Empty, item.myItem.explane);
        else if (item.myItem.itemTag == SlotTag.Weapon)
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, item.myItem.Att.ToString(), item.myItem.explane);
        else
            inventoryInfo.Item_InfoSetting(item.myItem.sprite, item.myItem.itemname, item.myItem.itemTag, item.myItem.Def.ToString(), item.myItem.explane);
    }
    public void SetItemInfoNull()
    {
        inventoryInfo.Item_InfoNull();
    }

    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        switch (tag)
        {
            case SlotTag.Head:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Head]);
                break;
            case SlotTag.Chest:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Chest]);
                break;
            case SlotTag.Legs:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Legs]);
                if (_amorPart[(int)SlotTag.Legs].Equals(string.Empty))
                    _playerEquipemntInfo.UnderwearSetActive(true);
                else
                    _playerEquipemntInfo.UnderwearSetActive(false);
                break;
            case SlotTag.Feet:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Feet]);
                break;
            case SlotTag.Gloves:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Gloves]);
                break;
            case SlotTag.Shoulders:
                EquipAmor(item, ref _amorPart[(int)SlotTag.Shoulders]);
                break;
            case SlotTag.Weapon:
                EquipWeapon(item);
                break;
        }
    }

    public void EquipAmor(InventoryItem item, ref string partStr)
    {
        if (item == null)
        {
            SetEquipment(_playerEquipemntInfo._amorObjectDic[partStr], false);
            partStr = string.Empty;
        }
        else
        {
            partStr = item.myItem.equipmentStr;
            SetEquipment(_playerEquipemntInfo._amorObjectDic[partStr], true);
        }
    }

    public void EquipWeapon(InventoryItem item)
    {
        if(item == null)
        {
            _playerEquipemntInfo._player.WeaponEquip(null);
        }
        else
        {
            var obj = _playerEquipemntInfo._amorObjectDic[item.myItem.equipmentStr];
            _playerEquipemntInfo._player.WeaponEquip(obj);
        }
    }

    public void SetEquipment(GameObject obj, bool set)
    {
        obj.SetActive(set);
        // 장비 효과 player에 주기
    }


    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item;
        if(_item == null)
        { _item = PickRandomItem(); }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem != null
                 && _item.sprite == inventorySlots[i].myItem.myItem.sprite 
                  && inventorySlots[i].myItem.currentCount < inventorySlots[i].myItem.myItem.MaxNumber)
            {
                inventorySlots[i].myItem.increaseCount(1);
                break;
            }
            // Check if the slot is empty
            else if(inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }
 
    void Close_Inventory()
    {
        SetItemInfoNull();
        transform.parent.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
