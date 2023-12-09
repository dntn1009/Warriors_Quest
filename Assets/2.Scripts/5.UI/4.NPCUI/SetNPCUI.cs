using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNPCUI : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI TalkText;
    [SerializeField] GameObject TalkObj;

    [Header("Edit string")]
    [SerializeField] string Name;
    [SerializeField] string Talk;

    Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        NameText.text = Name;
        TalkText.text = Talk;
        TalkObj.SetActive(false);
    }

    void Update()
    {
        SetUI();
    }

    #region [Methods]
    void SetUI()
    {
        this.transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
    }

    public void  TalkObjSetActive(bool set)
    {
        TalkObj.SetActive(set);
    }

    #endregion [Methods]

}
