using System.Collections;
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
    
    public int slotIndex = -1;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetFormationSlot(GameObject character)
    {
        if (character == null) return;

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

        level.text = $"Level{CharacterInfo.Level}";

        GameObject Model = Resources.Load<GameObject>($"CharacterModel/{CharacterInfo.Id}");
        var go = Instantiate(Model, modelContent);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -30f, 0f);
        go.GetComponent<RectTransform>().localScale = new Vector3(150f, 150f, 150f);
    }

    public void DelectFormationSlot()
    {

    }
}
