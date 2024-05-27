using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class NewCharacterInfo : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public CharacterData characterData;

    public Image panelImage;
    
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI runText;
    public TextMeshProUGUI skillText;
    public TextMeshProUGUI levelText;

    public Transform modelContent;

    public void SetCharacterDataUI(CharacterInfo ci)
    {
        foreach(Transform t in modelContent)
        {
            Destroy(t.gameObject);
        }

        characterInfo = ci;

        var go = Resources.Load("CharacterModel/" + ci.Id.ToString()) as GameObject;
        var model = Instantiate(go, modelContent.transform);
        model.transform.localScale = new Vector3(100f, 100f, 100f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);

        levelText.text = $"Level{ci.Level}";
        attackText.text = $"공격력 : {ci.Atk} +{ci.Atk_Up}";
        runText.text = $"도망 확률 : {ci.Run} -{ci.Run_Up}";

        if (characterInfo.Skill_Id == 0)
        {
            skillText.text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(characterInfo.Skill_Id.ToString());
            skillText.text = $"{data.Desc}";
        }
    }

    public void SetCharacterDataUI(CharacterData cd)
    {
        characterData = cd;

        var go = Resources.Load("CharacterModel/" + characterData.Id.ToString()) as GameObject;
        var model = Instantiate(go, modelContent.transform);
        model.transform.localScale = new Vector3(100f, 100f, 100f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);

        levelText.text = $"Level{characterData.Level}";
        attackText.text = $"공격력 : {characterData.Atk} +{characterData.Atk_Up}";
        runText.text = $"도망 확률 : {characterData.Run} -{characterData.Run_Up}";

        if (cd.Skill_Id == 0)
        {
            skillText.text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(cd.Skill_Id.ToString());
            skillText.text = $"{data.Desc}";
        }

    }

    private void OnDisable()
    {
        characterInfo = null;
    }

}
