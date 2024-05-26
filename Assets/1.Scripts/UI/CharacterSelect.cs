using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterSelect : MonoBehaviour
{
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

    public static event Action<CharacterInfo> OnCharacterUIClick;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        UpdateCharacterUI();
        //SceneManager.sceneLoaded += AwakeCharacterUI;
        exitButton.onClick.AddListener(OnExitButtonClick);
        OnCharacterUIClick += UpdateCharacterDescUI;
    }

    public void OnExitButtonClick()
    {
        UIManager.Instance.OpenUI(Page.FORMATION2);
    }

    public void AwakeCharacterUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            Debug.Log("AwakeCharacterUIMain");
            UpdateCharacterUI();
        }
    }

    public void UpdateCharacterUI()
    {
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
            go.GetComponent<CharacterNewSlot>().SetCharacterSlot(character.GetComponent<CharacterInfo>());
        }
    }

    public void UpdateCharacterDescUI(CharacterInfo ci)
    {
        characterDescTexts[(int)Desc.TIER].text = $"티어 : {ci.Tier}";
        characterDescTexts[(int)Desc.ATTACK].text = $"공격력 : {ci.Atk}";
        characterDescTexts[(int)Desc.RUN].text = $"도망 확률 : {ci.Run}%";

        if (ci.Skill_Id == 0)
        {
            characterDescTexts[(int)Desc.SKILL].text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(ci.Skill_Id.ToString());
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
}
