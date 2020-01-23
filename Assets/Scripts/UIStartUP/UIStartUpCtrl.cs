using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIStartUpCtrl : UICtrlBase
{
    public Slider m_loadingBar;
    public Text m_progress;
    public Button m_startBtn;
    public ENUIType m_uiType;

    private float m_add = 0.2f;

    void Update()
    {
        if(m_loadingBar.normalizedValue < 1f)
        {
            m_loadingBar.value += Time.deltaTime;
        }
    }
}
