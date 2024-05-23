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
        // 저장된 씬 이름을 가져옵니다.
        string sceneToLoad = PlayerPrefs.GetString("NextScene", "DefaultSceneName");

        // 가져온 씬 이름으로 씬을 비동기적으로 로드합니다.
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
