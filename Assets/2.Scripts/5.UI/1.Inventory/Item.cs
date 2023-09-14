using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1.Scriptable Object/UI/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public int MaxNumber;
    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab;
}
