using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
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
            case 1:
            case 2:
                audio = AudioClipList[0];
                break;
            case 3:
                audio = AudioClipList[1];
                break;
            case 4:
            case 5:
                audio = AudioClipList[2];
                break;
            case 6:
                audio = AudioClipList[3];
                break;
            case 7:
            case 8:
                audio = AudioClipList[4];
                break;
            case 9:
                audio = AudioClipList[5];
                break;
            case 10:
            case 11:
                audio = AudioClipList[6];
                break;
            case 12:
                audio = AudioClipList[7];
                break;
            default:
                audio = AudioClipList[0];
                break;
        }

        AudioSource.loop = true;
        AudioSource.volume = 0.03f;
        AudioSource.PlayOneShot(audio);
    }
}
