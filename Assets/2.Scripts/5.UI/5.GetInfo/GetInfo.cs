using System.Collections.Generic;
using UnityEngine;

public class GetInfo : MonoBehaviour
{
    [SerializeField] GameObject getText;
    [SerializeField] float finish = 2f;
    float time = 0;

    Queue<GameObject> getque;

    private void Awake()
    {
        getque = new Queue<GameObject>();
    }
    private void Update()
    {
        if (getque.Count > 0)
            getTextDelete();
    }

    public void setQueText(string text)
    {
        if (getque.Count > 5)
            Destroy(getque.Dequeue());

        var obj = Instantiate(getText);
        obj.transform.SetParent(transform);
        var get = obj.GetComponent<GetInfoText>();
        get.SetText(text);
        getque.Enqueue(obj);
    }

    public void getTextDelete()
    {
        time += Time.deltaTime;
        Debug.Log("Time : " + time);
        if (time >= finish)
        {
            time = 0;
            Destroy(getque.Dequeue());
        }
    }

}
