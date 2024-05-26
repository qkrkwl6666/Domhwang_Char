using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterNewSlot : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Transform modelContent;

    public TextMeshProUGUI levelText;
    public Button characterButton;

    public CharacterSelect characterSelect;

    private void Awake()
    {
        characterButton = GetComponent<Button>();

        characterButton.onClick.AddListener(OnCharacterButtonClick);

        characterSelect = GetComponentInParent<CharacterSelect>();
    }

    public void SetCharacterSlot(CharacterInfo ci)
    {
        characterInfo = ci;

        GameObject Model = Resources.Load<GameObject>($"CharacterModel/{characterInfo.Id}");
        var go = Instantiate(Model, modelContent);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -30f, 0f);
        levelText.text = $"Level{ci.Level}";
    }

    public void OnCharacterButtonClick()
    {
        characterSelect.UpdateCharacterDescUI(characterInfo);
    }

}
