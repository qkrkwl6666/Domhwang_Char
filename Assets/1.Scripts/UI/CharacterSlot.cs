using System;
using TMPro;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Sprite characterImage;
    public TextMeshProUGUI levelText;

    public static event Action<CharacterInfo> OnCharacterUIInfo;
    public static event Action<CharacterInfo, CharacterSlot> OnCharacterUISelect;

    private void OnEnable()
    {
        //if (characterInfo == null) return;
        //levelText.text = characterInfo.Level.ToString();
    }

    public void SetData(CharacterInfo data)
    {
        characterInfo = data;

        characterInfo.Id = data.Id;
        characterInfo.Name = data.Name;
        characterInfo.Tier = data.Tier;
        characterInfo.Atk = data.Atk;
        characterInfo.Atk_Up = data.Atk_Up;
        characterInfo.Run = data.Run;
        characterInfo.Run_Up = data.Run_Up;
        characterInfo.Skill_Id = data.Skill_Id;
        characterInfo.Level = data.Level;

        levelText.text = data.Level.ToString();
    }

    public void CharacterOnClick()
    {
        OnCharacterUIInfo?.Invoke(characterInfo);
        OnCharacterUISelect?.Invoke(characterInfo, this);
    }
}
