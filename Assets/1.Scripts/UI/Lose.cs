using System.Collections;
using System.Collections.Generic;
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
        GameManager.Instance.GameManagerAwake();
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.TITLE);
        
    }
}
