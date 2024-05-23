using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    public Button goMainMenuButton;

    private void Awake()
    {
        goMainMenuButton.onClick.AddListener(OnClickOpenMainMenu);
    }

    public void OnClickOpenMainMenu()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        //SceneManager.LoadScene("Main");
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        UIManager.Instance.OpenUI(Page.TITLE);
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/MainMenu"));
        if (GameManager.Instance.gameRestart)
        {
            GameManager.Instance.GameRestart();
        }
        
    }
}
