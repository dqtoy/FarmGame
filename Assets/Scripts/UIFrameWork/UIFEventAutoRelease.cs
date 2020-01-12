using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFEventAutoRelease : MonoBehaviour
{
    protected List<IRelease> m_releaseIDs = new List<IRelease>();

    public virtual void OnDestroy()
    {
        RemoveAllListeners();
    }

    public void AutoRelease(IRelease release)
    {
        m_releaseIDs.Add(release);
    }
    
    private void RemoveAllListeners()
    {
        foreach(var release in m_releaseIDs)
        {
            release.Release();
        }
    }
}
