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
    [field: SerializeField] public int BattleAttack { get; set; }
    [field: SerializeField] public int Atk_Effect_Id { get; set; }
    [field: SerializeField] public int Cry_Effect_Id { get; set; }
    [field: SerializeField] public int Run_Effect_Id { get; set; }

    [field: SerializeField] public CharacterSkillData CharacterSkillData { get; set; }

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
        BattleAttack = data.Atk;
        Atk_Effect_Id = data.Atk_Effect_Id;
        Cry_Effect_Id = data.Cry_Effect_Id;
        Run_Effect_Id = data.Run_Effect_Id;

        CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
        //characterImage = Resources.Load<Sprite>("ChatacterImage/" + Id.ToString() + "Img");
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
        BattleAttack = characterInfo.Atk;
        Atk_Effect_Id = characterInfo.Atk_Effect_Id;
        Cry_Effect_Id = characterInfo.Cry_Effect_Id;
        Run_Effect_Id = characterInfo.Run_Effect_Id;

        CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
        //CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
    }

    public CharacterData ConvertCharacterData()
    {
        CharacterData characterData = new CharacterData();

        characterData.Id = Id;
        characterData.Name = Name;
        characterData.Tier = Tier;
        characterData.Atk = Atk;
        characterData.Atk_Up = Atk_Up;
        characterData.Run = Run;
        characterData.Run_Up = Run_Up;
        characterData.Skill_Id = Skill_Id;
        characterData.Level = Level;
        characterData.Instance_Id = InstanceId;
        characterData.Atk_Effect_Id = Atk_Effect_Id;
        characterData.Cry_Effect_Id = Atk_Effect_Id;
        characterData.Run_Effect_Id = Atk_Effect_Id;

        return characterData;
    }

    public void LevelUp()
    {
        Level++;
        Atk += Atk_Up;
        Run -= Run_Up;

        if (Run < 0) Run = 0;
    }

}
