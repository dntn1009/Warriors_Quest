using DefineHelper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    public static ShopWindow Singleton;
    [SerializeField] PlayerController _player;

    [Header("Window && Button")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI myGold;
    [SerializeField] Button BuyBtn;
    [SerializeField] Button SellBtn;
    [SerializeField] ShopMouseInfo shopMouseInfo;

    [Header("Background")]
    [SerializeField] GameObject Buy;
    [SerializeField] GameObject Sell;

    [Header("Color")]
    [SerializeField] Color trueColor;
    [SerializeField] Color falseColor;

    [Header("Buy")]
    [SerializeField] BuyList[] buylist;
    [SerializeField] Sprite NoneImg;
    [SerializeField] Image buySprite;
    [SerializeField] GameObject Amountobj;
    [SerializeField] TMP_InputField Amount;
    [SerializeField] TextMeshProUGUI allCost;
    [SerializeField] TextMeshProUGUI costNotify;
    [SerializeField] Button costBtn;
    Item selectItem;

    [Header("Sell")]
    [SerializeField] sellInvenSlot[] sellInven_Slot;
    [SerializeField] Image sellSprite;
    [SerializeField] GameObject sellAmountobj;
    [SerializeField] TMP_InputField sellAmount;
    [SerializeField] TextMeshProUGUI sellCost;
    [SerializeField] TextMeshProUGUI sellNotify;
    [SerializeField] Button sellBtn;
    int sellMaxCount;
    int Slotnum;
    InventorySlot sellSlot;

    [Header("Header")]
    public bool hideCheck = false;
    public Vector2 truePos = new Vector2(0, 0);
    public Vector2 falsePos = new Vector2(2000, 1000);

    void Awake()
    {
        Singleton = this;
        sellMaxCount = 1;
        Slotnum = -1;
        Amount.onValueChanged.AddListener(buyValueChanged);
        sellAmount.onValueChanged.AddListener(sellValueChanged);
        costBtn.onClick.AddListener(BuyItem);
        sellBtn.onClick.AddListener(SellItem);
        Amountobj.SetActive(false);
        sellAmountobj.SetActive(false);
    }

    #region [BUY Methods]
    public void buyListSelect(Item _item)
    {
        buyCostNotifys("", Color.white);

        selectItem = _item;
        buySprite.sprite = selectItem.sprite;

        if (_item.itemTag == SlotTag.None || _item.itemTag == SlotTag.Potion)
        {
            Amountobj.SetActive(true);
            Amount.text = "1";
            allCost.text = (int.Parse(Amount.text) * _item.gold).ToString();
        }
        else
        {
            Amountobj.SetActive(false);
            allCost.text = _item.gold.ToString();
        }
    }

    public void buyValueChanged(string text)
    {
        if (!text.Equals(string.Empty))
            allCost.text = (int.Parse(text) * selectItem.gold).ToString();
        else
            allCost.text = "0";
    }
    public void BuyItem()
    {
        if (Amount.text.Equals("0"))
        {
            buyCostNotifys("수량을 확인해 주세요.", Color.white);
            return;
        }

        if (selectItem != null && _player._stat.GOLD >= int.Parse(allCost.text))
        {
            if (selectItem.itemTag == SlotTag.None || selectItem.itemTag == SlotTag.Potion)
                Inventory.Singleton.SpawnInventoryItem(selectItem, int.Parse(Amount.text));
            else
                Inventory.Singleton.SpawnInventoryItem(selectItem);

            _player._stat.GOLD -= int.Parse(allCost.text);
            myGold.text = _player._stat.GOLD.ToString();
            buyComplete();
            buyCostNotifys("구매 하였습니다.", Color.white);

            for (int i = 0; i < sellInven_Slot.Length; i++)
            {
                if (Inventory.Singleton.SlotInvenItem(i) != null)
                    sellInven_Slot[i].itemSetting(Inventory.Singleton.SlotInven(i), i);
            }

        }
        else if (selectItem != null && _player._stat.GOLD <= int.Parse(allCost.text))
        {
            buyCostNotifys("골드가 부족합니다.", Color.red);
        }
        else
        {
            buyCostNotifys("아이템을 선택해 주세요.", Color.white);
        }
    }

    public void buyCostNotifys(string text, Color color)
    {
        costNotify.text = text;
        costNotify.color = color;
    }

    #endregion [BUY Methods]


    #region [SELL Methods]
    public void sellInvenSlotSelect(InventorySlot _Slot, int num)
    {
        sellCostNotifys("", Color.white);
        sellSlot = _Slot;
        Slotnum = num;
        sellSprite.sprite = sellSlot.myItem.myItem.sprite;
        if (sellSlot.myItem.myItem.itemTag == SlotTag.None || sellSlot.myItem.myItem.itemTag == SlotTag.Potion)
        {
            sellMaxCount = sellSlot.myItem.currentCount;
            sellAmountobj.SetActive(true);
            sellAmount.text = sellSlot.myItem.currentCount.ToString();
            sellCost.text = ((sellSlot.myItem.currentCount) * (sellSlot.myItem.myItem.gold / 2)).ToString();
        }
        else
        {
            sellMaxCount = 1;
            sellAmountobj.SetActive(false);
            sellCost.text = (sellSlot.myItem.myItem.gold / 2).ToString();
        }
    }

    public void sellValueChanged(string text)
    {
        if (!text.Equals(string.Empty))
        {
            if (sellMaxCount < int.Parse(text))
            {
                sellAmount.text = sellMaxCount.ToString();
                sellCost.text = (sellSlot.myItem.currentCount * (sellSlot.myItem.myItem.gold / 2)).ToString();
                return;
            }
            sellCost.text = ((int.Parse(text)) * (sellSlot.myItem.myItem.gold / 2)).ToString();
        }
        else
            sellCost.text = "0";
    }
    public void SellItem()
    {
        if (sellAmount.text.Equals("0"))
        {
            sellCostNotifys("수량을 확인해 주세요.", Color.white);
            return;
        }
        if (sellSlot != null)
        {

            if (sellSlot.myItem.myItem.itemTag == SlotTag.None || sellSlot.myItem.myItem.itemTag == SlotTag.Potion)
            {
                if (sellSlot.myItem.currentCount == int.Parse(sellAmount.text))
                {
                    sellSlot.SellitemDestroy();
                    sellInven_Slot[Slotnum].itemNull(NoneImg);
                    _player._stat.GOLD += int.Parse(sellCost.text);
                    sellComplete();
                }
                else
                {
                    sellSlot.myItem.decreaseCount(int.Parse(sellAmount.text));
                    sellInven_Slot[Slotnum].itemRefresh();
                    sellMaxCount = sellInven_Slot[Slotnum].countNum();
                    _player._stat.GOLD += int.Parse(sellCost.text);
                    sellAmount.text = "1";
                    sellCost.text = (sellSlot.myItem.myItem.gold / 2).ToString();
                }
            }
            else
            {
                sellSlot.SellitemDestroy();
                sellInven_Slot[Slotnum].itemNull(NoneImg);
                _player._stat.GOLD += int.Parse(sellCost.text);
                sellComplete();
            }
            Inventory.Singleton.SetGoldInfo();
            sellCostNotifys("판매하였습니다.", Color.white);
        }
        //문제 1. decreaseCount 하고나서 판매창 인벤토리 리프레쉬가 힘들다.
        //문제 2. 아이템을 없애야되는데 InventoryItem에서 해결해야한다.
        
    }
    public void sellCostNotifys(string text, Color color)
    {
        sellNotify.text = text;
        sellNotify.color = color;
    }
    #endregion [SELL Methods]

    #region [Window Open Methods]
    public void buySlotSetting(NPCData npc)
    {
        title.text = npc.NAME + "의 상점";
        myGold.text = _player._stat.GOLD.ToString();

        for (int i = 0; i < npc.shopItem.Length; i++)
        {
            buylist[i].gameObject.SetActive(true);
            buylist[i].SetItemInfo(npc.shopItem[i]);
        }
    }

    public void sellSlotSetting(Inventory inventory)
    {
        for (int i = 0; i < sellInven_Slot.Length; i++)
        {
            if (inventory.SlotInvenItem(i) != null)
                sellInven_Slot[i].itemSetting(inventory.SlotInven(i), i);
        }
    }

    public void buySlotNull()
    {
        for (int i = 0; i < buylist.Length; i++)
        {
            if (!buylist[i]._nullCheck)
                buylist[i].SetitemNull();
            else
                break;
        }
        buyComplete();
    }

    public void sellSlotNull()
    {
        for (int i = 0; i < sellInven_Slot.Length; i++)
        {
            sellInven_Slot[i].itemNull(NoneImg);
        }
        sellComplete();
    }

    public void buyComplete()
    {
        selectItem = null;
        buySprite.sprite = NoneImg;
        Amount.text = string.Empty;
        Amountobj.SetActive(false);
        allCost.text = "0";
        buyCostNotifys("", Color.white);
    }

    public void sellComplete()
    {
        sellSlot = null;
        sellSprite.sprite = NoneImg;
        sellCost.text = "0";
        sellMaxCount = 1;
        Slotnum = -1;
        sellAmountobj.SetActive(false);

    }

    public void mouseInfoSetting(Item item, Vector2 pos)
    {
        shopMouseInfo.gameObject.SetActive(true);
        shopMouseInfo.GetComponent<RectTransform>().position = pos;
        shopMouseInfo.mouseInfoSetting(item);
    }
    public void mouseInfoFalse()
    {
        shopMouseInfo.gameObject.SetActive(false);
    }

    #endregion [Window Open Methods]

    #region [Window Select Button Methods]
    public void BuyActiveTrue()
    {
        BuyBtn.GetComponent<Image>().color = trueColor;
        SellBtn.GetComponent<Image>().color = falseColor;
        Buy.SetActive(true);
        Sell.SetActive(false);


    }

    public void SellActiveTrue()
    {
        SellBtn.GetComponent<Image>().color = trueColor;
        BuyBtn.GetComponent<Image>().color = falseColor;
        Sell.SetActive(true);
        Buy.SetActive(false);
    }
    #endregion [Window Select Button Methods]

}
