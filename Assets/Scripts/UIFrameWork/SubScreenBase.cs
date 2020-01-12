using UnityEngine;

public class SubScreenBase
{
    UISubCtrlBase subCtrlBase = null;
    public UISubCtrlBase SubCtrlBase { get => subCtrlBase; }
    public SubScreenBase(UISubCtrlBase subCtrl)
    {
        subCtrlBase = subCtrl;
        Init();
    }

    protected virtual void Init()
    {

    }

    protected virtual void Dispose()
    {

    }
}
