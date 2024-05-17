using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Tier { get; set; }
    [field: SerializeField] public int Atk { get; set; }
    [field: SerializeField] public int Atk_Up { get; set; }
    [field: SerializeField] public int Run { get; set; }
    [field: SerializeField] public int Run_Up { get; set; }
    [field: SerializeField] public int Skill_Id { get; set; }
    [field: SerializeField] public int InstanceId { get; set; }
    [field: SerializeField] public int Texture { get; set; }
    [field: SerializeField] public int Level { get; set; }

    public Sprite characterImage;
    public System.DateTime creationTime;

    public void SetCharacterData(CharacterData data)
    {
        Id = data.Id;
        Name = data.Name;
        Tier = data.Tier;
        Atk = data.Atk;
        Atk_Up = data.Atk_Up;
        Run = data.Run;
        Run_Up = data.Run_Up;
        Skill_Id = data.Skill_Id;
        Level = data.Level;
        Texture = data.Id;
        characterImage = Resources.Load<Sprite>("ChatacterImage/" + Id.ToString() + "Img");
    }

    public void SetCharacterInfo(CharacterInfo characterInfo)
    {
        Id = characterInfo.Id;
        Name = characterInfo.Name;
        Tier = characterInfo.Tier;
        Atk = characterInfo.Atk;
        Atk_Up = characterInfo.Atk_Up;
        Run = characterInfo.Run;
        Run_Up = characterInfo.Run_Up;
        Skill_Id = characterInfo.Skill_Id;
        Level = characterInfo.Level;
        Texture = characterInfo.Id;
        InstanceId = characterInfo.InstanceId;

        characterImage = Resources.Load<Sprite>("ChatacterImage/" + Id.ToString() + "Img");
    }

    public void LevelUp()
    {
        Level++;
        Atk += Atk_Up;
        Run -= Run_Up;

        if (Run < 0) Run = 0;
    }

    private void Awake()
    {

    }

}
