using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUIAnimation : MonoBehaviour
{
    [Header("Edit Param")]
    [SerializeField] AnimationCurve _opacityCurve; //투명성
    [SerializeField] AnimationCurve _scaleCurve; // 글자 크기
    [SerializeField] AnimationCurve _heightCurve; // 위로 움직임

    //참조 변수
    Camera _camera;

    //정보 변수
    TextMeshProUGUI _tmp; // Text
    float time = 0; // 시간 1f
    Vector3 origin; // 초기 위치

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
        _tmp.color = new Color(1, 1, 1, _opacityCurve.Evaluate(time)); // 점점 보이도록 함.
        transform.localScale = new Vector3(0.001f, 0.001f, 1) * _scaleCurve.Evaluate(time); // 커졋다 작아지도록 함.
        transform.position = origin + new Vector3(0,_heightCurve.Evaluate(time), 0); // 위치가 위로 올라감.
        time += Time.deltaTime;
    }
}
