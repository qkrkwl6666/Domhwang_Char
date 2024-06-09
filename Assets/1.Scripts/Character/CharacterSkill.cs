using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class CharacterSkill : MonoBehaviour
{
    public CharacterInfo CharacterInfo { get; set; }

    // 1 ��° ��ų �ߵ�
    public bool ActivateFirstSkill(BattleSystem battleSystem)
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
            var targets = SelectTargets(battleSystem, CharacterInfo.CharacterSkillData.Target);
            var skillApply = SkillType(battleSystem);

            foreach (var target in targets)
            {
                skillApply(target);
            }

            return true;
        }
        return false;
    }

    // 2 ��° ��ų �ߵ�
    public bool ActivateSecondSkill(BattleSystem battleSystem)
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
            var targets = SelectTargets(battleSystem, CharacterInfo.CharacterSkillData.Target);
            var skillApply = SkillType(battleSystem);

            if (skillApply == null || targets == null) return false;

            foreach (var target in targets)
            {
                skillApply(target);
            }

            return true;
        }

        return false;

    }

    public Action<GameObject> SkillType(BattleSystem battleSystem)
    {
        switch (CharacterInfo.CharacterSkillData.EffectType)
        {
            case 1:
                return target => IncreasedDamage(target.GetComponent<CharacterInfo>());
            case 2:
                return target => ChargeForward(target.GetComponent<CharacterControll>());
            case 3:
                return target => RemainingRoundsPass(target.GetComponent<CharacterInfo>());
            case 4:
                TryCountPass();
                break;
            case 5:
                // �ݵ�� ���� �ϸ� �ݵ�� �ܷ� ���� ����
                return target => ChargeAndRun(target.GetComponent<CharacterControll>());
            case 6:
                // �� ������ ���ݷ���, ���� �ο� �� ���� ���� ��ġ�� �����ȴ�.
                return target => HighDamage(battleSystem, target.GetComponent<CharacterInfo>());
        }

        return null;
    }

    public List<GameObject> SelectTargets(BattleSystem battleSystem, int targetType)
    {
        switch (targetType)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                return new List<GameObject> { gameObject };
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                return battleSystem.remainingCharacters;
            case 3:
                // ��� �ο����� ȿ�� ����
                return battleSystem.battleCharacter.SelectMany(x => x).ToList();
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                return battleSystem.roundsCharacters[nextRound - 1];
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                return battleSystem.roundsCharacters[battleSystem.CurrentRound - 1];
        }

        return null;
    }

    public void IncreasedDamage(CharacterInfo ci)
    {
        var skill = ci.gameObject.GetComponent<CharacterSkill>();
        ci.BattleAttack = skill.DamageCheck(CharacterInfo);
        ci.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
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
    public void ChargeForward(CharacterControll cc)
    {
        cc.isPass = true;
        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
    }

    public void RemainingRoundsPass(CharacterInfo ci)
    {
        var cc = ci.gameObject.GetComponent<CharacterControll>();
        cc.isAttackEndPass = true;
        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(ci.CharacterSkillData.Skill_Icon.ToString());
    }

    public void ChargeAndRun(CharacterControll cc)
    {
        cc.isPass = true;
        cc.confirmAttackEndRun = true;
        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
    }

    public void HighDamage(BattleSystem battleSystem, CharacterInfo chracterinfo)
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

        chracterinfo.BattleAttack = maxAttack;
        chracterinfo.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterInfo.CharacterSkillData.Skill_Icon.ToString());
    }


    public void TryCountPass()
    {
        GameManager.Instance.isPassTryCount = true;
    }
}
