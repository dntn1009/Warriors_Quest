using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DefineHelper;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] Slider LoadingValue;
    [SerializeField] Image LoadingPanel;

    Vector3 falsePos;
    Vector3 truePos;
    int length = 0;

    private void Awake()
    {
        falsePos = new Vector3(0, -1500, 0);
        truePos = new Vector3(0, 0, 0);
    }

    private void Update()
    {
      
    }

    public void SetLoading()
    {
        LoadingPanel.GetComponent<RectTransform>().anchoredPosition = truePos;
        if (this.gameObject.activeSelf)
            StartCoroutine(Loading(3f));
    }

    #region [LoadingText Methods]
    void LoadingMove()
    {
        if (length >= 3)
        {
            LoadingText.text = "GAME LOADING.";
            length = 0;
        }
        else
        {
            LoadingText.text += ".";
            length++;
        }
    }
    public void startLoading()
    {
        InvokeRepeating("LoadingMove", 0.1f, 0.75f);
    }
    public void finishLoading()
    {
        LoadingPanel.GetComponent<RectTransform>().anchoredPosition = falsePos;
        LoadingValue.value = 0f;
        CancelInvoke("LoadingMove");
    }
    #endregion [LoadingText Methods]

    #region [Coroutine Methods]

    IEnumerator Loading(float cooltime)
    {
        startLoading();
        float Initcool = cooltime;
        while (cooltime >= 0f)
        {
            cooltime -= Time.deltaTime;
            LoadingValue.value = 1 - (cooltime / Initcool);
            yield return new WaitForFixedUpdate();
        }
        finishLoading();
    }

    #endregion [Coroutine Methods]
}
