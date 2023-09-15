using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    Image itemIcon;
    TextMeshProUGUI _countText;

    int _currentCount;
    public int currentCount { get { return _currentCount; } set { _currentCount = value; } }
    public CanvasGroup canvasGroup { get; private set; }
    public Item myItem { get; set; }
    public InventorySlot activeSlot { get; set; }

    void Awake()
    {
        _countText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent)
    {
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.sprite;
        currentCount = 1;
    }

    public void increaseCount(int num)
    {
        currentCount += num;
        setCount();
    }

    public void decreaseCount(int num)
    {
        currentCount -= num;
        setCount();
    }

    void setCount()
    {
        if (currentCount == 1)
        {
            _countText.text = currentCount.ToString();
            _countText.gameObject.SetActive(false);
        }
        else if(currentCount > 1)
        {
            _countText.text = currentCount.ToString();
            _countText.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }
}
