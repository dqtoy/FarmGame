using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase
{
    public GameObject panelRoot = null;
    protected UICtrlBase ctrlBase = null;
    private string uiBaseName = string.Empty;
    public ScreenBase(string uiName)
    {
        StartLoad(uiName);
    }

    public virtual void StartLoad(string uiName)
    {
        uiBaseName = uiName;
        // param ??
        
        // load ui
        ResourceManager.GetInstance().LoadAsset(uiName, OnLoadComplete);
    }

    public void OnLoadComplete(GameObject go)
    {
        panelRoot = GameObject.Instantiate(go, GameUIManager.GetInstance().GetUIParent());
        // panelRoot.name = panelRoot.name.Replace("(Clone)","");

        OnLoadSuccess();
    }

    public void OnLoadSuccess()
    {

    }
}
