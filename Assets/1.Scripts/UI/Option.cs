using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public Main main;
    public Slider backgroundVolumeSlider;
    public Slider vfxVolumeSlider;
    public Button exitButton;

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

        exitButton.onClick.AddListener(ExitButtonOnClick);
    }

    public void ExitButtonOnClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        main.bossContent.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }


}
