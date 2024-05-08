using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButton);
        optionButton.onClick.AddListener(OptionButton);
        exitButton.onClick.AddListener(ExitButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartButton()
    {
        UIManager.instance.OpenUI(Page.STAGE);
    }

    private void OptionButton()
    {
        UIManager.instance.OpenUI(Page.OPTION);
    }

    private void ExitButton()
    {
        Application.Quit();
    }
}
