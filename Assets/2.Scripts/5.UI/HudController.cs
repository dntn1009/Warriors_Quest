using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] Image _hpBar; // HPBAR
    [SerializeField] Image _hpRateBar; // HPBAR �ȿ� �ִ� Foreground
    [SerializeField] float _Speed = 2f; // HPBAR�� �������� �Ծ��� �ÿ� �����̴� �ӵ�
    [SerializeField] TextMeshProUGUI _nameText;

    float _hpRate; // HPRATE�� �޾ƿ��� float

    Camera _camera; // MainCamera

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        SetHPBar();
    }

    #region [HP Methods]
    public void SetHPBar()
    {
        this.transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        _hpRateBar.fillAmount = Mathf.MoveTowards(_hpRateBar.fillAmount, _hpRate, _Speed * Time.deltaTime);
    }

    public void InitHPBar()
    {
        _hpBar.gameObject.SetActive(false);
        _hpRate = 1;
        _hpRateBar.fillAmount = 1f;
    }

    public void InitName(string _name)
    {
        _nameText.text = _name;
    }

    public void UpdateHPBar(float _current, float _max)
    {
        _hpRate = _current / _max;
        ActiveHPBar();
    }

    public void ShowHPBar() // �ǰݽ� HPBar ����
    {
        _hpBar.gameObject.SetActive(true);
    }

    public void HideHPBar() // Ư�� ��Ȳ�� ������ HPBAR �ȶ��.
    {
        _hpBar.gameObject.SetActive(false);
    }

    public void ActiveHPBar() //HPBAR TRUE �� �� �ʵ� HPBAR FALSE
    {
        ShowHPBar();
        if (IsInvoking("HideHPBar"))
            CancelInvoke("HideHPBar");
        Invoke("HideHPBar", 4f);
    }

    #endregion [HP Methods]
}
