using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class NextPortal : MonoBehaviour
{
    [SerializeField] MapType _type;
    [SerializeField] Vector3 _start;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DataManager.Instance._loadingmanager.SetLoading();
            IngameManager.Instance.ChangeMapFromMapType(_type);
            other.GetComponent<PlayerController>().NextMapPosition(_start);
        }
    }
}
