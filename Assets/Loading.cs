using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        // 가져온 씬 이름으로 씬을 비동기적으로 로드합니다.
        //SceneManager.LoadSceneAsync(GameManager.Instance.Sc);
    }
}
