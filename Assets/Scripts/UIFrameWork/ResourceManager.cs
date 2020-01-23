using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{    public void LoadAsset(string uiName, Action<GameObject> action)
    {
        GameObject go = Resources.Load<GameObject>(uiName);
        action?.Invoke(go);
    }
}
