using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        // ����� �� �̸��� �����ɴϴ�.
        string sceneToLoad = PlayerPrefs.GetString("NextScene", "DefaultSceneName");

        // ������ �� �̸����� ���� �񵿱������� �ε��մϴ�.
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
