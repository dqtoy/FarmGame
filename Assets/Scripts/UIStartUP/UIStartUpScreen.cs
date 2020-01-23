using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartUpScreen : ScreenBase
{
    UIStartUpCtrl startUpCtrl = null;

    public UIStartUpScreen() : base(UIConst.uiStartUp)
    {
    }

    protected override void StartLoad(string uiName)
    {
        uiType = startUpCtrl.m_uiType;
    }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        startUpCtrl = ctrlBase as UIStartUpCtrl;
        
    }   
}
