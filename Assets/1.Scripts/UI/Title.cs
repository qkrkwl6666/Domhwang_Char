using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    private void Update()
    {
        if(MultiTouchManager.Instance.Tap)
        {
            UIManager.Instance.OpenUI(Page.MAIN);
        }
    }
}
