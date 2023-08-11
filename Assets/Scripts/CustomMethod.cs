using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomMethod
{
    public static void SetChildrenActive(this Transform parent, bool active)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(active);
        }
    }
}
