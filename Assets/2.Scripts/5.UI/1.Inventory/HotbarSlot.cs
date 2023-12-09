using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] Image _potiomImg;
    [SerializeField] TextMeshProUGUI _number;

    public void SettingHotbar(Sprite img, string number)
    {
        _potiomImg.gameObject.SetActive(true);
        _potiomImg.sprite = img;
        _number.text = number;
    }

    public void SettingFalse()
    {
        _potiomImg.gameObject.SetActive(false);
    }
}
