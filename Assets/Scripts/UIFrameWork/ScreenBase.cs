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

    protected void OnLoadComplete(GameObject go)
    {
        panelRoot = GameObject.Instantiate(go, GameUIManager.GetInstance().GetUIParent());
        ctrlBase = panelRoot.GetComponent<UICtrlBase>();

        OnLoadSuccess();

        // 添加到控制层
        GameUIManager.GetInstance().AddUI(this);
    }

    protected virtual void OnLoadSuccess()
    {

    }

    public virtual void Close()
    {
        GameUIManager.GetInstance().RemoveUI(this);
    }

    public virtual void Dispose()
    {
        GameObject.Destroy(panelRoot);
    }
}
