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
    public ENScreenPriority m_screenPriority = ENScreenPriority.Default;

    void Awake()
    {
        m_ctrlCanvas = GetComponent<Canvas>();
    }
}
