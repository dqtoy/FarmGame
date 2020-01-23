using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUIType
{
    None,
    Main,
    UI,
}
public class GameUIManager : MonoSingleton<GameUIManager>
{
    // private GameObject m_gameMain; // 挂背景
    // private ScreenBase m_curMain;

    private GameObject m_uiRoot;
    public int m_uiOpenOrder = 0;
    private Camera m_uiCamera;
    public Camera UICamera { get => m_uiCamera; }
    private Dictionary<Type, ScreenBase> m_UIs = new Dictionary<Type, ScreenBase>();

    protected override void Init()
    {
        //     m_gameMain = Instantiate(Resources.Load("GameMain"), transform) as GameObject;
        //     m_gameMain.transform.SetParent(transform);

        m_uiRoot = Instantiate(Resources.Load("UIRoot"), transform) as GameObject;
        m_uiRoot.transform.SetParent(transform);

        m_uiCamera = m_uiRoot.GetComponent<Canvas>().worldCamera;
    }

    // public void AddMain(ScreenBase sBase)
    // {
    //     sBase.panelRoot.transform.SetParent(GetMainParent());
    //     sBase.panelRoot.name = sBase.panelRoot.name.Replace("(Clone)", "");
    // }

    // public void OpenMain()
    // {

    // }

    // 只是把UI挂到GameManager上
    public void AddUI(ScreenBase sBase)
    {
        sBase.panelRoot.transform.SetParent(GetUIParent());

        sBase.panelRoot.transform.localPosition = Vector3.zero;
        sBase.panelRoot.transform.localScale = Vector3.one;

        sBase.panelRoot.name = sBase.panelRoot.name.Replace("(Clone)", "");
    }

    public ScreenBase OpenUI(Type uiName, OpenScreenParameterBase param = null)
    {
        m_uiOpenOrder++;

        ScreenBase sb = GetUI(uiName);
        if (null != sb)
        {
            if (null != sb.ctrlBase && !sb.ctrlBase.m_ctrlCanvas.enabled)
            {
                sb.ctrlBase.m_ctrlCanvas.enabled = true;
            }
            return sb;
        }

        sb = Activator.CreateInstance(uiName, param) as ScreenBase;

        m_UIs.Add(uiName, sb);
        sb.SetOpenOrder(m_uiOpenOrder);

        return sb;
    }

    public ScreenBase GetUI(Type uiName)
    {
        ScreenBase sb;
        if (m_UIs.TryGetValue(uiName, out sb))
        {
            ;
        }

        return sb;
    }

    public void CloseUI(Type uiName)
    {

    }

    public void RemoveUI(ScreenBase sBase)
    {
        if (m_UIs.ContainsKey(sBase.GetType()))
            m_UIs.Remove(sBase.GetType());

        sBase.Dispose();
    }

    public Transform GetUIParent()
    {
        return transform;
    }

    // public Transform GetMainParent()
    // {
    //     return m_gameMain.transform;
    // }
}
