using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public Transform content;
    private List<Button> stageButton = new List<Button>();

    private void Awake()
    {
        foreach(Transform button in content)
        {
            var tempButton = button.GetComponent<Button>();
            stageButton.Add(tempButton);
            tempButton.onClick.AddListener(OpenForamtion);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenForamtion()
    {
        UIManager.instance.OpenUI(Page.FORMATION);
    }
}
