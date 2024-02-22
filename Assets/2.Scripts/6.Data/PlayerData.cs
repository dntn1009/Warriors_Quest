using DefineHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //날짜 및 스텟
    public string date;
    public Stat stat;

    public SaveItem[] invenSlot;
    public SaveItem[] hotbarSlot;
    public SaveItem[] equipSlot;

    //Map 및 위치
    public MapType map;
    public Vector3 position;

    public PlayerData()
    {

    }
    public PlayerData(Stat _stat, Inventory inven, MapType _type, Vector3 _pos)
    {
        date = DateTime.Now.ToString(("yyyy-MM-dd"));
        invenSlot = inven.SetsaveSlots(inven.inventorySlots);
        hotbarSlot = inven.SetsaveSlots(inven.hotbarSlots);
        equipSlot = inven.SetsaveSlots(inven.equipmentSlots);
        stat = _stat;
        stat.SKILLATTACK = 0;
        map = _type;
        position = _pos;
    }

    public PlayerData(Stat _stat, MapType _type, Vector3 _pos)
    {
        date = DateTime.Now.ToString(("yyyy-MM-dd"));
        stat = _stat;
        map = _type;
        position = _pos;
    }
    public PlayerData(string name, StatData _stat, MapType _type, Vector3 _pos)
    {
        date = DateTime.Now.ToString(("yyyy-MM-dd"));
        stat = new Stat(name, _stat.level, _stat.hpmax, _stat.hpmax, _stat.mpmax, _stat.mpmax, _stat.attack, 0, _stat.criattack, _stat.crirate, _stat.defence, _stat.dodgerate, _stat.hitrate, 0, _stat.expmax, 1500);
        invenSlot = new SaveItem[30];
        hotbarSlot = new SaveItem[7];
        equipSlot = new SaveItem[7];
        map = _type;
        position = _pos;
    }
}
