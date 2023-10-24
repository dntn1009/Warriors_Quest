using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "1.Scriptable Object/Skill")]
public class SkillData : ScriptableObject
{
    public GameObject _fxSkillPrefab; // Skill Effect
    public float _demage; // Demage
    public float _coolTime; // Seconds
}
