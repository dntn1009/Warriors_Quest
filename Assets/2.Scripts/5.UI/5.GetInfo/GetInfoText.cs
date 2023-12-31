using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetInfoText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI getText; // 텍스트
    [SerializeField] AnimationCurve _opacityCurve; // 투명성
    float time = 0;
    void Update()
    {
        SetTextAnimation();
    }

    void SetTextAnimation()
    {
        getText.color = new Color(1, 1, 1, _opacityCurve.Evaluate(time));
        time += Time.deltaTime;
    }

    public void SetText(string text)
    {
        getText.text = text;
        getText.color = Color.white;
    }
}
