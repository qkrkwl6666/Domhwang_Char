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
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));
        UIManager.Instance.OpenUI(Page.STAGE);
    }

    private void OptionButton()
    {
        //UIManager.Instance.OpenUI(Page.OPTION);
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        UIManager.Instance.OpenUI(Page.RULEBOOK);
    }

    private void ExitButton()
    {
        Application.Quit();
    }
}
