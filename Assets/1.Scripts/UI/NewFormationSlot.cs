using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewFormationSlot : MonoBehaviour
{
    public CharacterInfo CharacterInfo;
    public List<Sprite> LineSprites = new List<Sprite>();
    public Transform modelContent;
    public TextMeshProUGUI level;
    public Image image;
    public GameObject add;
    
    public int slotIndex = -1;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI runText;
    public TextMeshProUGUI skillText;

    private void Awake()
    {
        
    }

    public void SetFormationSlot(GameObject character)
    {
        if (character == null)
        {
            attackText.text = "";
            runText.text = "";
            skillText.text = "";
            level.text = "";
            image.sprite = LineSprites[0];
            foreach (Transform t in modelContent)
            {
                Destroy(t.gameObject);
            }
            add.SetActive(true);
            return;
        }

        CharacterInfo = character.GetComponent<CharacterInfo>();

        switch(CharacterInfo.Tier)
        {
            case "normal":
                image.sprite = LineSprites[0];
                break;
            case "rare":
                image.sprite = LineSprites[1];
                break;
            case "epic":
                image.sprite = LineSprites[2];
                break;
        }

        add.SetActive(false);

        level.text = $"Level{CharacterInfo.Level}";

        foreach(Transform t in modelContent)
        {
            Destroy(t.gameObject);
        }

        GameObject Model = Resources.Load<GameObject>($"CharacterModel/{CharacterInfo.Id}");
        var go = Instantiate(Model, modelContent);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -30f, 0f);
        go.GetComponent<RectTransform>().localScale = new Vector3(150f, 150f, 150f);

        attackText.text = $"공격력 : {CharacterInfo.Atk}";
        runText.text = $"도망 확률 : {CharacterInfo.Run}";

        if (CharacterInfo.Skill_Id == 0)
        {
            skillText.text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(CharacterInfo.Skill_Id.ToString());
            skillText.text = $"[{data.SkillName}] : {data.Desc}";
        }
    }

    public void DelectFormationSlot()
    {
        foreach (Transform t in modelContent)
        {
            Destroy(t.gameObject);
        }

        image.sprite = LineSprites[0];
        level.text = "";
        attackText.text = "";
        runText.text = "";
        skillText.text = "";

        add.SetActive(true);
    }
}
