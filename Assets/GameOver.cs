using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button gameOverButton;

    private void Awake()
    {
        gameOverButton.onClick.AddListener(GameOverButtonClick);
    }

    public void GameOverButtonClick()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.AudioSource.Stop();

        UIManager.Instance.OpenUI(Page.LOADING);
        GameManager.Instance.GameRestart();
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);

        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/MainMenu"));

        SceneManager.LoadScene("Main");
        
    }
}
