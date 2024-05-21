using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TryCount : MonoBehaviour
{
    public GameObject tryCount;
    public Transform Content;
    public TextMeshProUGUI stageText;

    public void OnEnable()
    {
        for(int i = 0; i < GameManager.Instance.TryCount; i++)
        {
            stageText.text = $"스테이지 {GameManager.Instance.CurrentStage + 1}";
            Instantiate(tryCount, Content);
        }
    }

    public void OnDisable()
    {
        foreach(Transform t in Content)
        {
            Destroy(t.gameObject);
        }
    }
}
