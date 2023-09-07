using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUIAnimation : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] AnimationCurve _opacityCurve; //����
    [SerializeField] AnimationCurve _scaleCurve; // ���� ũ��
    [SerializeField] AnimationCurve _heightCurve; // ���� ������

    //���� ����
    Camera _camera;

    //���� ����
    TextMeshProUGUI _tmp; // Text
    float time = 0; // �ð� 1f
    Vector3 origin; // �ʱ� ��ġ

    void Awake()
    {
        _camera = Camera.main;
        _tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    void Update()
    {
        SetDamageAnimation();
    }

    void SetDamageAnimation()
    {
        this.transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        _tmp.color = new Color(1, 1, 1, _opacityCurve.Evaluate(time)); // ���� ���̵��� ��.
        transform.localScale = new Vector3(0.001f, 0.001f, 1) * _scaleCurve.Evaluate(time); // Ŀ���� �۾������� ��.
        transform.position = origin + new Vector3(0,_heightCurve.Evaluate(time), 0); // ��ġ�� ���� �ö�.
        time += Time.deltaTime;
    }
}
