using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;

    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    [Header("PlayerEquipmentInfo")]
    [SerializeField] PlayerEquipmentInfo _playerEquipemntInfo;
    string _head;
    string _chest;
    string _legs;
    string _feets;
    string _gloves;
    string _shoulders;
    string _weapon;

    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener( delegate { SpawnInventoryItem(); } );
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

    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        switch (tag)
        {
            case SlotTag.Head:
                EquipAmor(item, ref _head);
                break;
            case SlotTag.Chest:
                EquipAmor(item, ref _chest);
                break;
            case SlotTag.Legs:
                EquipAmor(item, ref _legs);
                if (_legs.Equals(string.Empty))
                    _playerEquipemntInfo.UnderwearSetActive(true);
                else
                    _playerEquipemntInfo.UnderwearSetActive(false);
                break;
            case SlotTag.Feet:
                EquipAmor(item, ref _feets);
                break;
            case SlotTag.Gloves:
                EquipAmor(item, ref _gloves);
                break;
            case SlotTag.Shoulders:
                EquipAmor(item, ref _shoulders);
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


    public void UnEquipAmor(GameObject obj)
    {
        obj.SetActive(false);
    }

}
