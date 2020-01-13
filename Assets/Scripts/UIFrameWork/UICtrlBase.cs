using UnityEngine;
public enum ENScreenPriority
{
    Default = 0,
    PanelLayer = 10,
    PopLayer = 15,
}
public class UICtrlBase : UIFEventAutoRelease
{
    [HideInInspector]
    public Canvas m_ctrlCanvas;
    void Awake()
    {
        m_ctrlCanvas = GetComponent<Canvas>();
    }
}
