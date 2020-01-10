using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T m_instance;
    public static T GetInstance()
    {
        if(null == m_instance)
        {
            Type theType = typeof(T);
            m_instance = FindObjectOfType(theType) as T;
            
            if(null == m_instance)
            {
                var go = new GameObject(typeof(T).Name);
                m_instance = go.AddComponent<T>();

                GameObject bootObj = GameObject.Find("BootObj");
                if(null == bootObj)
                {
                    bootObj = new GameObject("BootObj");
                    DontDestroyOnLoad(bootObj);
                }
                go.transform.SetParent(bootObj.transform);
            }
        }
        
        return m_instance;
    }
    protected virtual void Awake()
    {
        if(null != m_instance && m_instance.gameObject != gameObject)
        {
            if(Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
        else if(null == m_instance)
        {
            m_instance = GetComponent<T>();
        }
        DontDestroyOnLoad(gameObject);
        Init();
    }

    protected virtual void Init()
    {

    }
}
