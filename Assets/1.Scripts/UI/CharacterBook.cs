using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterBook : MonoBehaviour
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
    public Button exitButton;
    public Transform characterUIContent;

    private void Awake()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
        CreateAllCharacter();
    }

    public void CreateAllCharacter()
    {
        foreach (CharacterData character in GameManager.Instance.AllCharacterDataList)
        {

            int index = 0;

            switch (character.Tier)
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
            go.GetComponent<CharacterDataSlot>().SetCreateCharacter(character);
        }
    }

    public void UpdateCharacterDescUI(CharacterData cd)
    {

        characterDescTexts[(int)Desc.TIER].text = $"이름 : {cd.Name}";
        characterDescTexts[(int)Desc.ATTACK].text = $"공격력 : {cd.Atk} (+{cd.Atk_Up})";
        characterDescTexts[(int)Desc.RUN].text = $"도망 확률 : {cd.Run}% (-{cd.Run_Up})";

        if (cd.Skill_Id == 0)
        {
            characterDescTexts[(int)Desc.SKILL].text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(cd.Skill_Id.ToString());
            characterDescTexts[(int)Desc.SKILL].text = $"스킬 : {data.Desc}";
        }
    }

    public void OnExitButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        UIManager.Instance.OpenUI(Page.MAIN);
    }
}
