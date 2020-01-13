using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoStart : MonoBehaviour
{
    void Start()
    {
        // GameUIManager.GetInstance().OpenUI(typeof(UILogoScreen));
        
        // TODO: hotfix
        GameUIManager.GetInstance().OpenUI(typeof(UIStartUpScreen));
    }
}
