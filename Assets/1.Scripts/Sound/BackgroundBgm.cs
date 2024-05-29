using System.Collections.Generic;
using UnityEngine;

public class BackgroundBgm : MonoBehaviour
{
    public AudioSource AudioSource;

    public List<AudioClip> AudioClipList;
    private void Awake()
    {
        AudioClip audio;

        switch (GameManager.Instance.CurrentStage)
        {
            case 0:
            case 1:
                audio = AudioClipList[0];
                break;
            case 2:
                audio = AudioClipList[1];
                break;
            case 3:
            case 4:
                audio = AudioClipList[2];
                break;
            case 5:
                audio = AudioClipList[3];
                break;
            case 6:
            case 7:
                audio = AudioClipList[4];
                break;
            case 8:
                audio = AudioClipList[5];
                break;
            case 9:
            case 10:
                audio = AudioClipList[6];
                break;
            case 11:
                audio = AudioClipList[7];
                break;
            default:
                audio = AudioClipList[0];
                break;
        }

        AudioSource.loop = true;
        AudioSource.volume = GameManager.Instance.BackgroundAudioSource.volume;
        AudioSource.PlayOneShot(audio);
    }
}
