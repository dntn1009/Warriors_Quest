using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Util
{
    #region [공용 함수]

    public static GameObject FindChildObject(GameObject parent, string childName)
    {
        var childList = parent.GetComponentsInChildren<Transform>();
        var results = childList.Where(obj => obj.name.Equals(childName));
        if (results != null)
            return results.First().gameObject;
        return null;
    }
    #endregion [공용 함수]
}
