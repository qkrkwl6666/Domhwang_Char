using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button guideButton;
    public Button characterButton;
    public Button formationButton;

    public Button nextButton;
    public Button prevButton;

    public int StageUI {  get; private set; }

    public TextMeshProUGUI bossName;
    public TextMeshProUGUI bossDesc;
    public TextMeshProUGUI stageDesc;

    public Transform bossContent;

    //public static event Action <MonsterData> OnMonsterData;

    private void OnEnable()
    {
        //OnMonsterData = null;

        BossUIUpdate();
    }

    private void Awake()
    {
        startButton.onClick.AddListener(StartButton);
        nextButton.onClick.AddListener(OnNextButtonClick);
        prevButton.onClick.AddListener(OnPrevButtonClick);
        formationButton.onClick.AddListener(OnFormationButtonClick);

        // optionButton.onClick.AddListener(OptionButton);
        // exitButton.onClick.AddListener(ExitButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        StageUI = GameManager.Instance.CurrentStage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPrevButtonClick()
    {
        GameManager.Instance.UiStage--;

        if(GameManager.Instance.UiStage < 0)
        {
            GameManager.Instance.UiStage = GameManager.Instance.MAX_STAGE - 1;
        }

        BossUIUpdate();
    }

    private void OnNextButtonClick()
    {
        GameManager.Instance.UiStage++;

        if (GameManager.Instance.UiStage >= GameManager.Instance.MAX_STAGE)
        {
            GameManager.Instance.UiStage = 0;
        }

        BossUIUpdate();
    }

    private void StartButton()
    {
        if (GameManager.Instance.UiStage != GameManager.Instance.CurrentStage) return;

        Debug.Log("시작");

        // GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        // GameManager.Instance.BackgroundAudioSource.Stop();
        // GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));
        // UIManager.Instance.OpenUI(Page.STAGE);
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

    public void OnFormationButtonClick()
    {
        UIManager.Instance.OpenUI(Page.FORMATION2);
    }

    public void BossUIUpdate()
    {
        foreach(Transform t in bossContent)
        {
            Destroy(t.gameObject);
        }

        if(GameManager.Instance.UiStage == GameManager.Instance.CurrentStage)
        {
            stageDesc.text = $"{GameManager.Instance.CurrentStage + 1} 스테이지";
            bossName.text = $"{GameManager.Instance.MonsterData.Name}";
            bossDesc.text = $"{GameManager.Instance.MonsterData.Desc}";

            GameObject Model = Resources.Load<GameObject>($"MonsterModel/{GameManager.Instance.MonsterData.Id}");
            var go = Instantiate(Model, bossContent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -100f, 0f);
            //OnMonsterData?.Invoke(GameManager.Instance.MonsterData);
        }
        else
        {
            stageDesc.text = $"{GameManager.Instance.UiStage + 1} 스테이지";
            bossName.text = $"{GameManager.Instance.AllMonsterData[GameManager.Instance.UiStage].Name}";
            bossDesc.text = $"{GameManager.Instance.AllMonsterData[GameManager.Instance.UiStage].Desc}";

            GameObject Model = Resources.Load<GameObject>($"MonsterModel/{GameManager.Instance.AllMonsterData[GameManager.Instance.UiStage].Id}");
            Instantiate(Model, bossContent);

            //OnMonsterData?.Invoke(GameManager.Instance.AllMonsterData[GameManager.Instance.UiStage]);
        }

    }
}
