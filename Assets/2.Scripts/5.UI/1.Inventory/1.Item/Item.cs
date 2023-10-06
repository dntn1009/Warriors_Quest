using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

[CreateAssetMenu(menuName = "1.Scriptable Object/UI/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public string itemname;
    public string explane;
    public int MaxNumber;
    public int money;

    [Header("If the item can be equipped")]
    public string equipmentStr;
    public int Att;
    public int Def;

    [Header("If the item can be healing")]
    public int hp;
    public int mp;
}
