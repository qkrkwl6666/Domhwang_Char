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
    [field: SerializeField] public int BattleAttack { get; set; }

    [field: SerializeField] public CharacterSkillData CharacterSkillData { get; set; }

    public Sprite characterImage;
    public System.DateTime creationTime;

    public BattleSystem battleSystem;

    public void ApplySkill(BattleSystem battleSystem)
    {
        if (CharacterSkillData == null) return;

        bool conditionMet = false;

        // 스킬 조건 체크
        switch (CharacterSkillData.ConditionType)
        {
            case 1:
                Debug.Log("스킬 1번 발동!!");
                conditionMet = battleSystem.MonsterInfo.Hp <= battleSystem.MonsterInfo.MaxHp * CharacterSkillData.ConditionValue * 0.01f;
                conditionMet = true;
                break;
            case 2:
                conditionMet = battleSystem.remainingCharacters.Count >= CharacterSkillData.ConditionValue;
                break;
            case 3:
                conditionMet = battleSystem.StandRemainingCharacters.Contains(gameObject);
                break;
            case 4:
                //conditionMet = battleSystem.playingCharacters.Count > 1;
                break;
            case 5:
                conditionMet = battleSystem.playingCharacters.Count == 1 && battleSystem.playingCharacters[0] == gameObject;
                break;
        }

        if (conditionMet)
        {
            switch (CharacterSkillData.EffectType)
            {
                case 1:
                    //Debug.Log($"스킬 발동 전 공격력 : {BattleAttack}");
                    BattleAttack = DamageCheck(this);
                    //Debug.Log($"스킬 발동 후 공격력 : {BattleAttack}");
                    break;
            }
        }

        // 스킬 타겟 선택 및 효과 적용
        switch (CharacterSkillData.Target)
        {
            case 1:
                // 자신의 캐릭터에게 효과 적용
                Debug.Log("자신의 캐릭터에게 효과 적용");
                break;
            case 2:
                // 잔류 병사에게 효과 적용
               Debug.Log("잔류 병사에게 효과 적용");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack += DamageCheck(cc);
                }
                break;
            case 3:
                // 모든 인원에게 효과 적용
                Debug.Log("모든 인원에게 효과 적용");
                foreach (var characterList in battleSystem.roundsCharacters)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterInfo>();
                        cc.BattleAttack += DamageCheck(cc);
                    }
                }
                break;
            case 4:
                // 다음 라운드의 돌격 인원에게 효과 적용
                //int nextRound = battleSystem.Round + 1;
                //if (nextRound <= battleSystem.roundsCharacters.Count)
                //{
                //    foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                //    {
                //        
                //    }
                //}
                break;
        }
    }

    public int DamageCheck(CharacterInfo characterInfo)
    {
        int damage = characterInfo.BattleAttack;

        switch (CharacterSkillData.EffectType)
        {
            case 1:
                if (CharacterSkillData.EffectValue.EndsWith("x"))
                {
                    float multiplier = float.Parse(CharacterSkillData.EffectValue.TrimEnd('x'));
                    damage *= (int)multiplier;
                }
                else
                {
                    damage += int.Parse(CharacterSkillData.EffectValue);
                }
                break;
        }

        return damage;
    }


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
        CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
        BattleAttack = data.Atk;

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

        return characterData;
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
