using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageSkillStatus
{
    None,
    FirstGuard,
    IsRemainingReduceDamage,
    IsInvincible,
    IsRoundIncreasedDamage,
}

public class MonsterSkill : MonoBehaviour
{
    MonsterInfo monsterInfo;

    public DamageSkillStatus currentDamageSkill = DamageSkillStatus.None;

    private void Awake()
    {
        monsterInfo = GetComponent<MonsterInfo>();
    }

    public bool InitialActivateSkill(BattleSystem battleSystem)
    {
        if (monsterInfo == null || monsterInfo.SkillData == null) return false;

        bool conditionMet = false;

        switch (monsterInfo.SkillData.ConditionType)
        {
            case 1:
                conditionMet = true;
                break;
        }

        if (conditionMet)
        {
            SkillType(battleSystem);
        }

        return false;
    }

    public bool RoundsActiveSkill(BattleSystem battleSystem)
    {
        if (monsterInfo == null || monsterInfo.SkillData == null) return false;

        bool conditionMet = false;

        switch (monsterInfo.SkillData.ConditionType)
        {
            case 1:
                conditionMet = true;
                break;
            // 해당라운드가 ConditionValue 라운드 일때 
            case 2:
                conditionMet = battleSystem.CurrentRound == monsterInfo.SkillData.ConditionValue;
                break;
        }

        if (conditionMet) SkillType(battleSystem);
        else if (currentDamageSkill != DamageSkillStatus.FirstGuard) currentDamageSkill = DamageSkillStatus.None;

        return false;
    }

    public void SkillType(BattleSystem battleSystem)
    {
        switch (monsterInfo.SkillData.EffectType)
        {
            case 1:
                Health();
                break;
            case 2:
                currentDamageSkill = DamageSkillStatus.IsRemainingReduceDamage;
                break;
            case 3:
                currentDamageSkill = DamageSkillStatus.IsInvincible;
                break;
            case 4:
                currentDamageSkill = DamageSkillStatus.IsRoundIncreasedDamage;
                break;
            case 5:
                currentDamageSkill = DamageSkillStatus.FirstGuard;
                break;
        }
    }

    public void Health()
    {
        monsterInfo.Hp += monsterInfo.SkillData.EffectValue;

        if(monsterInfo.Hp > monsterInfo.MaxHp) monsterInfo.Hp = monsterInfo.MaxHp;

        monsterInfo.hpSlider.value = monsterInfo.Hp;
    }

    public void RemainingReduceDamage(int damage)
    {
        int reducedDmg = damage - monsterInfo.SkillData.EffectValue;
        if (reducedDmg < 0) { reducedDmg = 0; }

        monsterInfo.Hp -= reducedDmg;

        Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
        DynamicTextManager.CreateText2D(position, reducedDmg.ToString(), DynamicTextManager.defaultData);
    }

    public void RoundIncreasedDamageReceived(int damage)
    {
        monsterInfo.Hp -= damage * monsterInfo.SkillData.EffectValue;
        Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
        DynamicTextManager.CreateText2D(position, (damage * 8).ToString(), DynamicTextManager.defaultData);
    }

    public void FirstAttackDefence()
    {
        Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
        DynamicTextManager.CreateText2D(position, "0", DynamicTextManager.defaultData);
        currentDamageSkill = DamageSkillStatus.None;
    }
}
