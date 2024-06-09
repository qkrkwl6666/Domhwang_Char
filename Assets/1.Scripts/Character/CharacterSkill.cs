using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    public CharacterInfo CharacterInfo { get; set; }

    public bool InitializeSkill(BattleSystem battleSystem)
    {
        if (CharacterInfo.CharacterSkillData == null) return false;

        bool conditionMet = false;

        switch (CharacterInfo.CharacterSkillData.ConditionType)
        {
            case 0:
                conditionMet = true;
                break;
            // �ش���尡 ConditionValue ���� �϶� 
            case 6:
                conditionMet = battleSystem.CurrentRound == CharacterInfo.CharacterSkillData.ConditionValue;
                break;

            // �ش� ���忡�� ĳ���Ͱ� ���ݿ� ���� �� ��
            case 9:
                var cc = GetComponent<CharacterControll>();
                cc.RunMode(true);
                conditionMet = cc.isRun == false;
                break;
        }

        if (conditionMet)
        {
            SkillType(battleSystem);
            return true;
        }
        return false;
    }

    public bool ApplySkill(BattleSystem battleSystem)
    {
        if (CharacterInfo.CharacterSkillData == null) return false;

        bool conditionMet = false;

        // ��ų ���� üũ
        switch (CharacterInfo.CharacterSkillData.ConditionType)
        {
            case 1:
                conditionMet = battleSystem.MonsterInfo.Hp <= battleSystem.MonsterInfo.MaxHp * CharacterInfo.CharacterSkillData.ConditionValue * 0.01f;
                break;
            case 2:
                conditionMet = battleSystem.remainingCharacters.Count >= CharacterInfo.CharacterSkillData.ConditionValue;
                break;
            case 3:
                conditionMet = battleSystem.StandRemainingCharacters.Contains(gameObject);
                break;
            case 4:
                // ĳ���Ͱ� �ܷ� ���� Ȯ�� �ϰ� && ���ÿ� �ٸ� �ο��� ������ �������� ���
                conditionMet = battleSystem.StandRemainingCharacters.Contains(gameObject) && battleSystem.StandRemainingCharacters.Count >= 2;
                break;
            case 5:
                conditionMet = battleSystem.playingCharacters.Count == 1 && battleSystem.playingCharacters[0] == gameObject;
                break;
            case 8:
                conditionMet = battleSystem.CurrentRound == CharacterInfo.CharacterSkillData.ConditionValue && GetComponent<CharacterControll>().isRun;
                break;
        }

        if (conditionMet)
        {
            SkillType(battleSystem);

            return true;
        }

        return false;

    }

    public void SkillType(BattleSystem battleSystem)
    {
        switch (CharacterInfo.CharacterSkillData.EffectType)
        {
            case 1:
                IncreasedDamage(battleSystem);
                break;
            case 2:
                chargeForward(battleSystem);
                break;
            case 3:
                RemainingRoundsPass(battleSystem);
                break;
            case 4:
                TryCountPass();
                break;
            case 5:
                // �ݵ�� ���� �ϸ� �ݵ�� �ܷ� ���� ����
                ChargeAndRun(battleSystem);
                break;
            case 6:
                // �� ������ ���ݷ���, ���� �ο� �� ���� ���� ��ġ�� �����ȴ�.
                HighDamage(battleSystem);
                break;
        }
    }

    public void IncreasedDamage(BattleSystem battleSystem)
    {
        // ��ų Ÿ�� ���� �� ȿ�� ����
        switch (CharacterInfo.CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����

                CharacterInfo.BattleAttack = DamageCheck(CharacterInfo);
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var ci = character.GetComponent<CharacterInfo>();
                    var skill = character.GetComponent<CharacterSkill>();
                    ci.BattleAttack = skill.DamageCheck(CharacterInfo);
                    ci.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var ci = character.GetComponent<CharacterInfo>();
                        var skill = character.GetComponent<CharacterSkill>();
                        ci.BattleAttack = skill.DamageCheck(CharacterInfo);
                        ci.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var ci = character.GetComponent<CharacterInfo>();
                    var skill = character.GetComponent<CharacterSkill>();
                    ci.BattleAttack = skill.DamageCheck(CharacterInfo);
                    ci.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var ci = character.GetComponent<CharacterInfo>();
                    var skill = character.GetComponent<CharacterSkill>();
                    ci.BattleAttack = skill.DamageCheck(CharacterInfo);
                    ci.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public int DamageCheck(CharacterInfo characterInfo)
    {
        float damage = 0;

        switch (characterInfo.CharacterSkillData.EffectType)
        {
            case 1:
                if (characterInfo.CharacterSkillData.EffectValue.EndsWith("x"))
                {
                    float multiplier = float.Parse(characterInfo.CharacterSkillData.EffectValue.TrimEnd('x'));
                    damage = (int)multiplier * characterInfo.BattleAttack;
                }
                else if (characterInfo.CharacterSkillData.EffectValue.EndsWith("%"))
                {
                    float multiplier = float.Parse(characterInfo.CharacterSkillData.EffectValue.TrimEnd('%'));
                    damage = (int)multiplier * characterInfo.BattleAttack * 0.01f;
                }
                else
                {
                    damage = int.Parse(characterInfo.CharacterSkillData.EffectValue) + CharacterInfo.BattleAttack;
                }
                break;
        }

        return (int)damage;
    }

    // ������ ���� �޼���
    public void chargeForward(BattleSystem battleSystem)
    {
        switch (CharacterInfo.CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����

                GetComponent<CharacterControll>().isPass = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����

                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����

                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isPass = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void RemainingRoundsPass(BattleSystem battleSystem)
    {
        switch (CharacterInfo.CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����

                GetComponent<CharacterControll>().isAttackEndPass = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����

                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����

                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isAttackEndPass = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void ChargeAndRun(BattleSystem battleSystem)
    {
        switch (CharacterInfo.CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����

                GetComponent<CharacterControll>().isPass = true;
                GetComponent<CharacterControll>().confirmAttackEndRun = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����

                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����

                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isPass = true;
                        cc.confirmAttackEndRun = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void HighDamage(BattleSystem battleSystem)
    {
        int maxAttack = 0;

        foreach (var characterList in battleSystem.battleCharacter)
        {
            foreach (var character in characterList)
            {
                var ci = character.GetComponent<CharacterInfo>();
                if (maxAttack < ci.BattleAttack) maxAttack = ci.BattleAttack;
            }
        }

        switch (CharacterInfo.CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����

                CharacterInfo.BattleAttack = maxAttack;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����

                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����

                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterInfo>();
                        cc.BattleAttack = maxAttack;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }
    public void TryCountPass()
    {
        GameManager.Instance.isPassTryCount = true;
    }
}
