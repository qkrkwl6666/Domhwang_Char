using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNewSlot : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Transform modelContent;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI roundTeamText;
    public Button characterButton;

    public CharacterSelect characterSelect;

    private void Awake()
    {
        roundTeamText.text = "";

        characterButton = GetComponent<Button>();

        characterButton.onClick.AddListener(OnCharacterButtonClick);

        characterSelect = GetComponentInParent<CharacterSelect>();
    }

    public void SetCharacterSlot(CharacterInfo ci)
    {
        characterInfo = ci;

        GameObject Model = Resources.Load<GameObject>($"CharacterModel/{characterInfo.Id}");
        var go = Instantiate(Model, modelContent);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);
        levelText.text = $"Level{ci.Level}";

        UpdateTeamTextUI();
    }

    public void UpdateTeamTextUI()
    {
        for (int i = 0; i < GameManager.Instance.formationCharacterList.Count; i++)
        {
            if (GameManager.Instance.formationCharacterList[i] == null) continue;

            switch (i)
            {
                case 0:
                case 1:
                case 2:
                    if (characterInfo == GameManager.Instance.formationCharacterList[i].GetComponent<CharacterInfo>())
                    {
                        roundTeamText.text = "1 ���� ��";
                        return;
                    }
                    break;
                case 3:
                case 4:
                    if (characterInfo == GameManager.Instance.formationCharacterList[i].GetComponent<CharacterInfo>())
                    {
                        roundTeamText.text = "2 ���� ��";
                        return;
                    }
                    break;
                case 5:
                    if (characterInfo == GameManager.Instance.formationCharacterList[i].GetComponent<CharacterInfo>())
                    {
                        roundTeamText.text = "3 ���� ��";
                        return;
                    }
                    break;
                default:
                    roundTeamText.text = "";
                    break;
            }
        }
        roundTeamText.text = "";
    }

    public void OnCharacterButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        characterSelect.UpdateCharacterDescUI(gameObject);
    }

}
