using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapWindow : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] PlayerController _player;

    [Header("PosXY")]
    [SerializeField] TextMeshProUGUI _PosXText;
    [SerializeField] TextMeshProUGUI _PosZText;


    public void SetPosXYInfo()
    {
        _PosXText.text = _player.transform.position.x.ToString();
        _PosZText.text = _player.transform.position.z.ToString();
    }
}
