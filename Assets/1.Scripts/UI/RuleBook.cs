using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleBook : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public Button exitButton;

    public readonly int MAX_PAGE = 6;

    public Image ruleImage;

    public int CurrentPage { get; private set; } = 0;

    public List<Sprite> ruleSprites = new List<Sprite>();

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
        prevButton.onClick.AddListener(OnPrevButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    public void OnNextButtonClick()
    {
        ++CurrentPage;

        if (CurrentPage > 5)
        {
            CurrentPage = 0;
        }
        ruleImage.sprite = ruleSprites[CurrentPage];

    }

    public void OnPrevButtonClick()
    {
        --CurrentPage;

        if (CurrentPage < 0)
        {
            CurrentPage = 5;
        }
        ruleImage.sprite = ruleSprites[CurrentPage];
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/MainMenu"));
        UIManager.Instance.OpenUI(Page.MAIN);
    }
}
