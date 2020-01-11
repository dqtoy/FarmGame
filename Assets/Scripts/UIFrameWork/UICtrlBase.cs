using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrlBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas m_ctrlCanvas;
    void Awake()
    {
        m_ctrlCanvas = GetComponent<Canvas>();
    }
}
