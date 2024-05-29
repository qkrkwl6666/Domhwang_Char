using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterSelect : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public NewFormation newFormation;
    enum Desc
    {
        TIER,
        ATTACK,
        RUN,
        SKILL,
    }

    public List<GameObject> characterUIPrefabs = new List<GameObject>();
    public List<TextMeshProUGUI> characterDescTexts = new List<TextMeshProUGUI>();

    public Transform characterUIContent;

    public Button exitButton;
    public Button cencelButton;
    public Button changeButton;

    public List<GameObject> characters = new List<GameObject>();

    //public static event Action<GameObject> OnCharacterUIClick;
    private void OnEnable()
    {
        UpdateCharacterUI();
    }

    private void Awake()
    {
        UpdateCharacterUI();
        //SceneManager.sceneLoaded += AwakeCharacterUI;
        exitButton.onClick.AddListener(OnExitButtonClick);
        //OnCharacterUIClick += UpdateCharacterDescUI;
        
        changeButton.onClick.AddListener(OnChangeButtonClick);
        cencelButton.onClick.AddListener(OnCencelButtonClick);
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        characterInfo = null;
        UIManager.Instance.OpenUI(Page.FORMATION2);
    }

    public void AwakeCharacterUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
          
            UpdateCharacterUI();
        }
    }

    public void UpdateCharacterUI()
    {
        foreach(Transform character in characterUIContent)
        {
            Destroy(character.gameObject);
        }

        characters.Clear();

        foreach (GameObject character in GameManager.Instance.PlayerCharacterList)
        {
            var cc = character.GetComponent<CharacterInfo>();

            int index = 0;

            switch (cc.Tier)
            {
                case "normal":
                    index = 0;
                    break;
                case "rare":
                    index = 1;
                    break;
                case "epic":
                    index = 2;
                    break;
            }

            var go = Instantiate(characterUIPrefabs[index], characterUIContent);
            characters.Add(go);
            go.GetComponent<CharacterNewSlot>().SetCharacterSlot(character.GetComponent<CharacterInfo>());
        }
    }

    public void UpdateCharacterShowTeam()
    {
        foreach(var character in characters)
        {
            character.GetComponent<CharacterNewSlot>().UpdateTeamTextUI();
        }
    }

    public void UpdateCharacterDescUI(GameObject go)
    {
        var cns = go.GetComponent<CharacterNewSlot>();

        characterInfo = cns.characterInfo;

        characterDescTexts[(int)Desc.TIER].text = $"이름 : {cns.characterInfo.Name}";
        characterDescTexts[(int)Desc.ATTACK].text = $"공격력 : {cns.characterInfo.Atk} (+{cns.characterInfo.Atk_Up})";
        characterDescTexts[(int)Desc.RUN].text = $"도망 확률 : {cns.characterInfo.Run}% (-{cns.characterInfo.Run_Up})";

        if (cns.characterInfo.Skill_Id == 0)
        {
            characterDescTexts[(int)Desc.SKILL].text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(cns.characterInfo.Skill_Id.ToString());
            characterDescTexts[(int)Desc.SKILL].text = $"스킬 : {data.Desc}";
        }
    }

    public void CharacterUIDestory(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle")
        {
            foreach(Transform t in characterUIContent)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public void OnChangeButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        if (characterInfo == null) return;

        int index = newFormation.selectedFormationSlot.slotIndex;

        for (int i = 0; i < newFormation.characterFormation.Count; i++)
        {
            var slot = newFormation.characterFormation[i].GetComponent<NewFormationSlot>();
            if (i == index) continue;
            else if (i != index && slot.CharacterInfo == characterInfo)
            {
                GameManager.Instance.formationCharacterList[i] = null;
                slot.DelectFormationSlot();
            }
        }

        GameManager.Instance.formationCharacterList[index] = characterInfo.gameObject;

        characterInfo = null;
        UIManager.Instance.OpenUI(Page.FORMATION2);
    }

    public void OnCencelButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        int index = newFormation.selectedFormationSlot.slotIndex;

        var slot = newFormation.characterFormation[index].GetComponent<NewFormationSlot>();
        GameManager.Instance.formationCharacterList[index] = null;
        slot.DelectFormationSlot();
        characterInfo = null;

        UIManager.Instance.OpenUI(Page.FORMATION2);
    }

}
