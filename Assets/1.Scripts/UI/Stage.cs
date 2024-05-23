using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public Transform content;
    private List<Button> stageButton = new List<Button>();

    private void OnEnable()
    {
        int stage = GameManager.Instance.CurrentStage;

        for (int i = 0; i < stageButton.Count; i++)
        {
            if (i == stage) stageButton[i].interactable = true;
            else stageButton[i].interactable = false; 
        }
    }

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
        UIManager.Instance.OpenUI(Page.FORMATION);
    }
}
