using UnityEngine;

public class UICtrlBase : UIFEventAutoRelease
{
    [HideInInspector]
    public Canvas m_ctrlCanvas;
    void Awake()
    {
        m_ctrlCanvas = GetComponent<Canvas>();
    }
}
