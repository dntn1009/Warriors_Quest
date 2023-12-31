using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetInfoText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI getText; // �ؽ�Ʈ
    [SerializeField] AnimationCurve _opacityCurve; // ����
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
