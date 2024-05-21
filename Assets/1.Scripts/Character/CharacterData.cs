using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

public class CharacterData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Tier { get; set; }
    public int Atk { get; set; }
    public int Atk_Up { get; set; }
    public int Run { get; set; }
    public int Run_Up { get; set; }
    public int Skill_Id { get; set; }
    public int Instance_Id { get; set; }
    public int Texture { get; set; }
    public int Level { get; set; }

    public void LevelUp()
    {
        Level++;
        Atk += Atk_Up;
        Run -= Run_Up;

        if (Run < 0) Run = 0;
    }

    public CharacterData GetCharacterData()
    {
        CharacterData data = new CharacterData();

        data.Id = Id;
        data.Name = Name;
        data.Tier = Tier;
        data.Atk = Atk;
        data.Atk_Up = Atk_Up;
        data.Run = Run;
        data.Run_Up = Run_Up;
        data.Skill_Id = Skill_Id;
        data.Instance_Id = Instance_Id;
        data.Texture = Texture;
        data.Level = Level;

        return data;
    }

}
