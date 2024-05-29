using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public Button gameRestartButton;

    private void Awake()
    {
        gameRestartButton.onClick.AddListener(GameRestartButton);
    }

    public void GameRestartButton()
    {
        UIManager.Instance.OpenUI(Page.LOADING);
        GameManager.Instance.GameRestart();
        SceneManager.LoadScene("Main");
    }
}
