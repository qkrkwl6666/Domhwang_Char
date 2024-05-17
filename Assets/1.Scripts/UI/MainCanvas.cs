using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
 
    }

    private void OnEnable()
    {
        canvas.worldCamera = Camera.main;
    }

}
