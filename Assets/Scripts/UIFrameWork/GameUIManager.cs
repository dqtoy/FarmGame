﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoSingleton<GameUIManager>
{
    private GameObject m_uiRoot;
    private Camera m_uiCamera;
    private Dictionary<Type, ScreenBase> m_UIs = new Dictionary<Type, ScreenBase>();

    protected override void Init()
    {
        m_uiRoot = Instantiate(Resources.Load("Prefab/UIRoot"), transform) as GameObject;
        m_uiCamera = m_uiRoot.GetComponent<Canvas>().worldCamera;
    }

    // 只是把UI挂到GameManager上
    public void AddUI(ScreenBase sBase)
    {
        sBase.panelRoot.transform.SetParent(GetUIParent());

        sBase.panelRoot.transform.localPosition = Vector3.zero;
        sBase.panelRoot.transform.localScale = Vector3.one;
    }

    public ScreenBase OpenUI(Type uiName)
    {
        ScreenBase sb = GetUI(uiName);
        if(null != sb)
        {
            return sb;
        }

        sb = Activator.CreateInstance(uiName) as ScreenBase;

        m_UIs.Add(uiName, sb);
        return sb;
    }

    public ScreenBase GetUI(Type uiName)
    {
        ScreenBase sb;
        if(m_UIs.TryGetValue(uiName, out sb))
        {
            ;
        }

        return sb;
    }

    public void CloseUI(Type uiName)
    {

    }

    public Transform GetUIParent()
    {
        return transform;
    }
}