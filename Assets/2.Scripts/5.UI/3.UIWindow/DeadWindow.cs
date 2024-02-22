using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;

public class DeadWindow : MonoBehaviour
{
    [Header("Home Position")]
    [SerializeField] Vector3 HomePos;

    [Header("UI")]
    [SerializeField] Button homeBtn;
    [SerializeField] PlayerController player;

    private void Awake()
    {
        homeBtn.onClick.AddListener(delegate { SetHomeBtn(); });
    }

    public void SetHomeBtn()
    {
        this.gameObject.SetActive(false);
        IngameManager.Instance.ChangeMapFromMapType(MapType.Stage1);
        player.NextMapPosition(HomePos);

    }
}
