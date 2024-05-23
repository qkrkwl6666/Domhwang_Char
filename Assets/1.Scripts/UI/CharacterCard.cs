using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{


    public GameObject Cardback;
    private CharacterData characterData;
    private Button button;

    public Transform character;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI runText;
    public TextMeshProUGUI skillText;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCardButton);
    }

    public void SetData(CharacterData characterData)
    {
        this.characterData = characterData.GetCharacterData();
    }

    public void OnCardButton()
    {
        GetComponentInParent<NewCharacter>().SetSelectCharacter(characterData);
        GetComponentInParent<NewCharacter>().OpenUIFormation();
    }

    public void CardAwake()
    {
        // 하드 코딩 바꿔야 함
        switch (GameManager.Instance.CurrentStage)
        {
            case 1:
            case 2:
                break;
            case 3:
            case 4:
            case 5:
                characterData.LevelUp();
                break;
            case 6:
            case 7:
            case 8:
                for (int i = 0; i < 2; i++)
                {
                    characterData.LevelUp();
                }
                break;
            case 9:
            case 10:
            case 11:
                for (int i = 0; i < 3; i++)
                {
                    characterData.LevelUp();
                }
                break;
        }

        nameText.text = characterData.Name;
        levelText.text = $"Lv{characterData.Level}";
        attackText.text = $"{characterData.Atk}";
        runText.text = $"{characterData.Run}";
        skillText.text = $"{characterData.Skill_Id}";

        switch (GameManager.Instance.TryCount)
        {
            case 1:
                Cardback.GetComponent<Image>().sprite = Resources.Load<Sprite>(Defines.CardPath + Defines.Card_Unknown);
                Cardback.SetActive(true);
                break;
            case 2:
                Cardback.SetActive(true);
                switch (characterData.Tier)
                {
                    case "normal":
                        Cardback.GetComponent<Image>().sprite = Resources.Load<Sprite>(Defines.CardPath + Defines.Card_Normal);
                        break;
                    case "rare":
                        Cardback.GetComponent<Image>().sprite = Resources.Load<Sprite>(Defines.CardPath + Defines.Card_Rare);
                        break;
                    case "epic":
                        Cardback.GetComponent<Image>().sprite = Resources.Load<Sprite>(Defines.CardPath + Defines.Card_Epic);
                        break;
                }

                
                //Cardback.GetComponent<Image>().color = GetTierColor(characterData.Tier);
                break;
            case 3:
                var go = Resources.Load("CharacterModel/" + characterData.Id.ToString()) as GameObject;

                var model = Instantiate(go, character);

                model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -50f, 0f);
                Cardback.SetActive(false);
                character.gameObject.GetComponent<Image>().color = GetTierColor(characterData.Tier);
                break;
        }
    }

    public Color GetTierColor(string tier)
    {
        switch (tier)
        {
            case "normal": return new Color(255f / 255f, 255f / 255f, 235f / 255f, 1);
            case "rare": return new Color(50f / 255f, 62f / 255f, 79f / 255f, 1);
            case "epic": return new Color(150f / 255f, 66f / 255f, 83f/ 255f, 1);
            default : return Color.white;
        }
    }
}
