using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewFormation : MonoBehaviour
{
    public List<Button> formationRoundButtons = new List<Button>();
    public List<GameObject> formationsUI = new List<GameObject>();

    public List<Button> characterFormation = new List<Button>();
    public NewFormationSlot selectedFormationSlot;

    public CharacterSelect characterSelect;

    public int selectIndex = -1;

    public Button exitButton;

    private void OnEnable()
    {
        for(int i = 0; i < characterFormation.Count; i++)
        {
            var nfs = characterFormation[i].GetComponent<NewFormationSlot>();
            nfs.SetFormationSlot(GameManager.Instance.formationCharacterList[i]);
        }
    }

    private void Awake()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);

        // Todo �ݺ������� 
        formationRoundButtons[0].onClick.AddListener(OnRound1ButtonClick);
        formationRoundButtons[1].onClick.AddListener(OnRound2ButtonClick);
        formationRoundButtons[2].onClick.AddListener(OnRound3ButtonClick);
    }

    public void OnButtonCharacterClick(NewFormationSlot newFormationSlot)
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        selectedFormationSlot = newFormationSlot;
        characterSelect.UpdateCharacterShowTeam();
        UIManager.Instance.OpenUI(Page.CHARACTERSELECT);
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        // �� ����
        GameManager.Instance.Save();

        UIManager.Instance.OpenUI(Page.MAIN);
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/MainMenu"));
    }

    public void OnRound1ButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        ActiveFormationUI(0);
    }

    public void OnRound2ButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        ActiveFormationUI(1);
    }

    public void OnRound3ButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        ActiveFormationUI(2);
    }

    public void ActiveFormationUI(int index)
    {
        for (int i = 0; i < formationsUI.Count; i++)
        {
            if (i == index) formationsUI[i].SetActive(true);
            else formationsUI[i].SetActive(false);
        }
    }
}
