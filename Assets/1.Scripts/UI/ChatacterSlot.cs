using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatacterSlot : MonoBehaviour
{
    public CharacterData characterData;
    public Sprite characterImage;

    public static event Action<CharacterData, Sprite> OnCharacterUIInfo;

    public void SetData(CharacterData data)
    {
        characterData.Id = data.Id;
        characterData.Name = data.Name;
        characterData.Tier = data.Tier;
        characterData.Atk = data.Atk;
        characterData.Atk_Up = data.Atk_Up;
        characterData.Run = data.Run;
        characterData.Run_Up = data.Run_Up;
        characterData.Skill_Id = data.Skill_Id;
    }

    public void CharacterOnClick()
    {
        OnCharacterUIInfo?.Invoke(characterData, characterImage);
    }
}
