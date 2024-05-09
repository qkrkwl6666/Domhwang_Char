using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatacterSlot : MonoBehaviour
{
    public CharacterInfo characterInfo;
    public Sprite characterImage;

    public static event Action<CharacterInfo> OnCharacterUIInfo;

    public void SetData(CharacterInfo data)
    {
        characterInfo.Id = data.Id;
        characterInfo.Name = data.Name;
        characterInfo.Tier = data.Tier;
        characterInfo.Atk = data.Atk;
        characterInfo.Atk_Up = data.Atk_Up;
        characterInfo.Run = data.Run;
        characterInfo.Run_Up = data.Run_Up;
        characterInfo.Skill_Id = data.Skill_Id;
    }

    public void CharacterOnClick()
    {
        OnCharacterUIInfo?.Invoke(characterInfo);
    }
}
