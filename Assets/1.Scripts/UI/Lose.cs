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
        //SceneManager.LoadScene("Main");
        
        UIManager.Instance.OpenUI(Page.TITLE);
        if(GameManager.Instance.gameRestart)
        {
            GameManager.Instance.GameRestart();
        }
        
    }
}
