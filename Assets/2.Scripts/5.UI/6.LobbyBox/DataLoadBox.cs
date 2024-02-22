using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataLoadBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LoadingText;
    [SerializeField] TextMeshProUGUI FailureText;
    [SerializeField] TextMeshProUGUI BackText;
    [SerializeField] Button QuitBtn;
    [SerializeField] Button BackBtn;
    

    private void Awake()
    {
        LoadingText.gameObject.SetActive(true);
        FailureText.gameObject.SetActive(false);
        QuitBtn.gameObject.SetActive(false);
        BackText.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(false);    
        QuitBtn.onClick.AddListener(delegate { Quit(); });
        BackBtn.onClick.AddListener(delegate { Back(); });

    }

    public void FailureLoad()
    {
        LoadingText.gameObject.SetActive(false);
        BackText.gameObject.SetActive(false);
        BackBtn.gameObject.SetActive(false);
        FailureText.gameObject.SetActive(true);
        QuitBtn.gameObject.SetActive(true);
    }
    public void BackLoad()
    {
        LoadingText.gameObject.SetActive(false);
        FailureText.gameObject.SetActive(false);
        QuitBtn.gameObject.SetActive(false);
        BackText.gameObject.SetActive(true);
        BackBtn.gameObject.SetActive(true);
    }

    void Quit()
    {
        Application.Quit();
    }

    void Back()
    {
        this.gameObject.SetActive(false);
        LobbyManager.Singleton.SetMenu();
    }
}
