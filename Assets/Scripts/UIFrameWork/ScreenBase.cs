using UnityEngine;

public class ScreenBase
{
    public GameObject panelRoot = null;
    public UICtrlBase ctrlBase = null;
    public int uiOpenOrder = 0;//界面打开顺序
    public int sortingLayer = 0;
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

    public void SetOpenOrder(int order)
    {
        uiOpenOrder = order;
        if(null != ctrlBase && null != ctrlBase.m_ctrlCanvas)
        {
            ctrlBase.m_ctrlCanvas.sortingOrder = uiOpenOrder;
        }
    }

    protected void OnLoadComplete(GameObject go)
    {
        panelRoot = GameObject.Instantiate(go, GameUIManager.GetInstance().GetUIParent());
        ctrlBase = panelRoot.GetComponent<UICtrlBase>();

        UpdateLayoutLevel();

        OnLoadSuccess();

        // 添加到控制层
        GameUIManager.GetInstance().AddUI(this);
    }

    private void UpdateLayoutLevel()
    {
        var camera = GameUIManager.GetInstance().UICamera;
        if(null != camera)
        {
            ctrlBase.m_ctrlCanvas.worldCamera = camera;
        }

        ctrlBase.m_ctrlCanvas.pixelPerfect = true;
        ctrlBase.m_ctrlCanvas.overrideSorting = true;
        ctrlBase.m_ctrlCanvas.sortingLayerID = (int)ctrlBase.m_screenPriority;
        // ctrlBase.m_ctrlCanvas.sortingOrder = uiOpenOrder;
    }
    
}
