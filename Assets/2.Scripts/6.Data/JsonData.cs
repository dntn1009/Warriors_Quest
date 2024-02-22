using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonData
{
    public List<StatData> _statData;
    public List<TalkData> _talkData;

    public JsonData() { }
    public JsonData(List<StatData> stat, List<TalkData> talk)
    {
        _statData = stat;
        _talkData = talk;
    }
}
