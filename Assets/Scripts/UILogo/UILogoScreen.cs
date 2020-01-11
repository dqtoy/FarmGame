using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogoScreen : ScreenBase
{
    UILogoCtrl logoCtrl = null;
    public UILogoScreen() : base(UIConst.uiLogo)
    {
        
    }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        logoCtrl = ctrlBase as UILogoCtrl;
    } 
}
