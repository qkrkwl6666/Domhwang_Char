using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Main main;
    public Slider backgroundVolumeSlider;
    public Slider vfxVolumeSlider;
    public Button exitButton;
    public Button gameRestartButton;
    public Button gameExitButton;

    private void Awake()
    {
        backgroundVolumeSlider.value = 0.1f;
        vfxVolumeSlider.value = 0.2f;

        GameManager.Instance.BackgroundAudioSource.volume = backgroundVolumeSlider.value;
        GameManager.Instance.AudioSource.volume = vfxVolumeSlider.value;

        backgroundVolumeSlider.onValueChanged.AddListener(delegate 
        { GameManager.Instance.BackgroundAudioSource.volume = backgroundVolumeSlider.value; });

        vfxVolumeSlider.onValueChanged.AddListener(delegate
        { GameManager.Instance.AudioSource.volume = vfxVolumeSlider.value; });

        exitButton.onClick.AddListener(OnExitButtonClick);
        gameRestartButton.onClick.AddListener(OnGameRestart);
        gameExitButton.onClick.AddListener(OnGameExitButtonClick);
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        main.bossContent.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnGameRestart()
    {
        UIManager.Instance.OpenUI(Page.LOADING);
        GameManager.Instance.GameRestart();
        SceneManager.LoadScene("Main");
    }

    public void OnGameExitButtonClick()
    {
        Application.Quit();
    }


}
