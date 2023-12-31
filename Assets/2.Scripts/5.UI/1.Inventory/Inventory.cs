using DefineHelper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [Header("Item Slots & Gold")]
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] InventorySlot[] hotbarSlots;
    // 0=Head, 1=Chest, 2=Legs, 3=Feet
    [SerializeField] InventorySlot[] equipmentSlots;
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
        SetGoldInfo();
    }

    void Update()
    {
        if (carriedItem == null) return;

        carriedItem.transform.position = Input.mousePosition;
    }


    #region [Item Carried Methods]
    public void SetCarriedItem(InventoryItem item) //아이템 옮기기
    {
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

        if (random <= num)
        {
            IngameManager.Instance.SetGetInfoText(items[num].itemname + "을(를) 획득하셨습니다.");
            SpawnInventoryItem(items[num]);
        }

    }
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
    }
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

    }

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

    }
    #endregion [Spawn Item Methods]
}
