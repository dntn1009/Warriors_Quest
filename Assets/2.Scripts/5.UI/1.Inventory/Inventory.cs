using DefineHelper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [Header("Item Slots & Gold")]
    public InventorySlot[] inventorySlots;
    public InventorySlot[] hotbarSlots;
    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    public InventorySlot[] equipmentSlots;
    [SerializeField] InventoryInfo inventoryInfo;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] TextMeshProUGUI goldText;
    [Header("Item List")]
    [SerializeField] Item[] items;

    [Header("Debug")]
    [SerializeField] Button giveItemBtn;

    [Header("PlayerEquip & EquipStat")]
    [SerializeField] EquipStat _equipStat;
    [SerializeField] PlayerEquipmentInfo _playerEquipemntInfo;

    public EquipStat EQUIPSTAT { get { return _equipStat; } set { _equipStat = value; } }

    public bool hideCheck = false;
    public Vector2 truePos = new Vector2(0, 0);
    public Vector2 falsePos = new Vector2(2000, 0);

    void Awake()
    {
        Singleton = this;
        giveItemBtn.onClick.AddListener(delegate { SpawnInventoryItem(null, 10); });
        _equipStat = new EquipStat();
    }

    void Update()
    {
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }


    #region [Item Carried Methods]
    public void SetCarriedItem(InventoryItem item) //아이템 옮기기
    {
        AudioManager.Instance.UiPlay(AudioManager.Instance.invenClick);
        bool nullCheck = true; // 누른 장비 슬롯이 NULL인지 아닌지 확인하는 bool

        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            nullCheck = false;
            item.activeSlot.SetItem(carriedItem, nullCheck);// (장착과정) 2.마우스로 템을 들고 있는 경우
        }

        if (item.activeSlot.myTag != SlotTag.None)
        {
            if (item.activeSlot.myTag == SlotTag.Potion)
                item.transform.parent.GetComponent<InventorySlot>().HotbarActiveFalse();
            else
                unequipEquipment(item, nullCheck); // (장착과정) 3. 장착한 장비 슬롯을 눌렀을 경우 해제 carrid에 옮김

        }
        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);

    }

    #endregion [Item Carried Methods]

    #region [Item EquipMent Methods]
    public void equipEquipMent(InventoryItem item, bool nullCheck)
    {
        _playerEquipemntInfo.equipEquipment(item, nullCheck);
        _equipStat += item.myItem.equipstat;
        IngameManager.Instance.equipMentStatInfo();
    }

    public void unequipEquipment(InventoryItem item, bool nullCheck)
    {
        _playerEquipemntInfo.unequipEquipment(item, nullCheck);
        _equipStat -= item.myItem.equipstat;
        IngameManager.Instance.equipMentStatInfo();
    }

    #endregion [Item EquipMent Methods]

    #region [Item Info Methods]
    public void SetItemInfo(InventoryItem item)
    {
        if (carriedItem != null)
            return;

        inventoryInfo.Item_InfoSetting(item);
    }

    public void SetItemInfoNull()
    {
        inventoryInfo.Item_InfoNull();
    }

    #endregion [Item Info Methods]

    #region [Item Healing Potion]

    public void UsePotionItemEffect(InventoryItem item)
    {
        var _player = _playerEquipemntInfo.gameObject.GetComponent<PlayerController>();
        _player.UsePotionItem(item);
    }

    public void keyUsePotion(int index)
    {
        UsePotionItemEffect(hotbarSlots[index].myItem);
        hotbarSlots[index].HotborSlotSettingHotbar();
    }

    #endregion [Item Healing Potion]

    #region [Gold Methods]
    public void SetGoldInfo()
    {
       goldText.text = _playerEquipemntInfo.GetComponent<PlayerController>()._stat.GOLD.ToString();
    }
    #endregion [Gold Methods]

    #region [Spawn Item Methods]

    public void SpawnInventoryItem(Item item, int _number = 1)
    {
        Item _item = item;

        if (_item == null)
        { _item = PickRandomItem(); }

        if (_item.itemTag == SlotTag.None || _item.itemTag == SlotTag.Potion)
        {
            bool SlotCheck = false;
            int EmptySlot = -1;
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].myItem == null && EmptySlot == -1)
                    EmptySlot = i;

                if (inventorySlots[i].myItem != null && _item == inventorySlots[i].myItem.myItem)
                {
                    inventorySlots[i].myItem.increaseCount(_number);
                    QuestCountSetting(inventorySlots[i]);
                    SlotCheck = true;
                    break;
                }
            }

            if (SlotCheck)
                return;
            else
            {
                bool CreateCheck = true;
                for (int i = 0; i < hotbarSlots.Length; i++)
                {
                    if (hotbarSlots[i].myItem != null && _item == hotbarSlots[i].myItem.myItem)
                    {
                        hotbarSlots[i].myItem.increaseCount(_number);
                        hotbarSlots[i].HotborSlotSettingHotbar();
                        QuestCountSetting(hotbarSlots[i]);
                        CreateCheck = false;
                        break;
                    }
                }
                //Hotbar에도 등록되어 있는지 확인해야한다.
                if (CreateCheck && EmptySlot != -1)
                {
                    Instantiate(itemPrefab, inventorySlots[EmptySlot].transform).Initialize(_item, inventorySlots[EmptySlot], _number);
                    QuestCountSetting(inventorySlots[EmptySlot]);
                }
            }
        }
        else
        {
            _number = 1;
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].myItem == null)
                {
                    Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i], _number);
                    QuestCountSetting(inventorySlots[i]);
                    break;
                }
            }
        }
    }

    Item PickRandomItem()
    {
        int random = Random.Range(0, items.Length);
        return items[random];
    }

    ////////////////////////////////
    public void GetRewardItem(Item item, int number = 1)
    {
        IngameManager.Instance.SetGetInfoText(item.itemname + "을(를) 획득");
        SpawnInventoryItem(item, number);
    }
    public void GetDropItem(int num, int number = 1)
    {
        IngameManager.Instance.SetGetInfoText(items[num].itemname + "을(를) 획득");
        SpawnInventoryItem(items[num], number);
    }
    public void GetRandomDropItem(int num, int rand)
    {
        int random = Random.Range(0, 100);

        if (random <= rand)
        {
            IngameManager.Instance.SetGetInfoText(items[num].itemname + "을(를) 획득하셨습니다.");
            SpawnInventoryItem(items[num]);
        }

    }
    //// 몬스터를 사냥하거나 퀘스트 클리어 시, Inventory에 아이템 획득, 오른쪽 중간UI에 획득 알림
    public void InitQuestCheck(PlayerController _player)
    {
        if (_player._quest.questGoal.questType == QuestType.Gathering)
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].myItem != null && inventorySlots[i].myItem.myItem.itemCode == _player._quest.questGoal.itemCode)
                {
                    _player._quest.questGoal.currentAmount = inventorySlots[i].myItem.currentCount;
                    break;
                }
            }
    } // Quest 수집형 시, 처음 수집할 아이템이 있는지 체크하기 위한 메서드
    public void QuestCountSetting(InventorySlot slot)
    {
        PlayerController _player = _playerEquipemntInfo.GetComponent<PlayerController>();
        if (_player._quest.isActive)
        {
            if (_player._quest.questGoal.questType == QuestType.Gathering && slot.myItem.myItem.itemCode == _player._quest.questGoal.itemCode)
            {
                _player._quest.questGoal.currentAmount = slot.myItem.currentCount;
                IngameManager.Instance.QuestRefresh();
            }
        }

    } // Quest 수집형 시, 개수 카운팅
    public void QuestDeliver(QuestData quest)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].myItem != null && inventorySlots[i].myItem.myItem.itemCode == quest.questGoal.itemCode)
            {
                if (inventorySlots[i].myItem.currentCount > quest.questGoal.requiredAmount)
                {
                    inventorySlots[i].myItem.decreaseCount(quest.questGoal.requiredAmount);
                }
                else
                {
                    inventorySlots[i].myItem = null;
                }
            }
        }

    } //Quest 클리어 시 Item 삭제 및 카운트 감소

    public InventoryItem SlotInvenItem(int number)
    {
        return inventorySlots[number].myItem;
    }

    public InventorySlot SlotInven(int number)
    {
        return inventorySlots[number];
    }
    #endregion [Spawn Item Methods]

    #region [Setting Inventory Data Methods]

    public SaveItem[] SetsaveSlots(InventorySlot[] slots)
    {
        SaveItem[] save = new SaveItem[slots.Length];

        for(int i = 0; i < slots.Length; i++)
        {
            if (!(slots[i].myItem == null || slots[i].myItem.myItem == null || slots[i].myItem.myItem.itemCode == 0))
                save[i] = new SaveItem(slots[i]);
            
        }

        return save;
    }

    public void LoadItemData(SaveItem[] Items, InventorySlot[] Slots)
    {
       for(int i = 0; i < Slots.Length; i++)
       {
            if (Items[i].itemCode == 0 || Items[i] == null)
                continue;
            else
            {
               for(int j = 0; j < items.Length; j++)
               {
                    if(items[j].itemCode == Items[i].itemCode)
                    {
                        Instantiate(itemPrefab, Slots[i].transform).Initialize(items[j], Slots[i], Items[i].Count);
                        if (Slots[i].myTag != SlotTag.None)
                        {
                            if (Slots[i].myTag == SlotTag.Potion)
                                Slots[i].hotbarSlot.SettingHotbar(Slots[i].myItem.myItem.sprite, Slots[i].myItem.CountStr);
                            else
                                equipEquipMent(Slots[i].myItem, true); // 여기가 장착 장소임. 장비 슬롯에 아이템의 유무에 따라 다르게 작동함.
                        }
                        break;
                    }
               }
            }

        }
        
    }

    #endregion [Setting Inventory Data Methods]
}
