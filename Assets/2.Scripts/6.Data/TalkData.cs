using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TalkData
{
    public int npcID;
    public int step;
    public int Length;
    public string str0;
    public string str1;
    public string str2;
    public string str3;
    public string str4;
    public string str5;

    public static TalkData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<TalkData>(jsonString);
    }

    public string[] setStrArr()
    {
        string[] talk = new string[Length];
        for (int i = 0; i < Length; i++)
            talk[i] = setStr(i);

        return talk;
    }

    public string setStr(int number)
    {
        if (number == 0)
            return str0;
        else if (number == 1)
            return str1;
        else if (number == 2)
            return str2;
        else if (number == 3)
            return str3;
        else if (number == 4)
            return str4;
        else
            return str5;
    }


}
