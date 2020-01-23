using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenuScreen : ScreenBase
{
    UIMainMenuCtrl uiMainMenuCtrl = null;
    public UIMainMenuScreen(OpenScreenParameterBase param = null) : base(UIConst.uiMainMenu, param)
    {

    }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        uiMainMenuCtrl = ctrlBase as UIMainMenuCtrl;
    }  


}
