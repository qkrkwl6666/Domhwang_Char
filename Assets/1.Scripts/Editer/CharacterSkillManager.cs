using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataTableMgr))]
public class CharacterSkillManager : Editor
{
    private string[] ConditionTypeStrings = new string[] 
    { 
        "조건 없음", "적의 체력이 ConditionValue이하 일때", "잔류 병사가 ConditionValue이상일 때 ",
        "현재 캐릭터가 잔류에 성공할 시", "함께 돌격에 성공한 인원이 있다면",
        "홀로 돌격에 성공 할 시", "ConditionValue 라운드 돌격 일때", "ConditionValue 라운드에서",
        "ConditionValue 라운드 에서 돌격 상태에서 도망갈 시", "해당 캐릭터가 돌격에 성공 할 때",
    };

    private string[] EffectTypeStrings = new string[]
    {
        "공격력 이 EffectValue 만큼 증가", "무조건 돌격" , "남은 라운드 잔류",
        "트라이 카운트 소모되지 않음", "반드시 돌격 하며 반드시 잔류 하지 않음",
        "편성된 인원 중 가장 높은 공격력 적용",
    };

    private string[] EffectDurationStrings = new string[]
    {
        "영구 지속",
    };

    private string[] SkillTargetStrings = new string[]
    {
        "자신의 캐릭터","잔류 병사","모든 인원","다음 라운드의 돌격 인원","해당 라운드의 캐릭터"
    };

    // 버튼 변수
    private string skillCreateButtonLabel = "캐릭터 스킬 생성";
    private bool skillCreateButtonShowDropdown = false;

    private static readonly string skillIdString = "스킬 아이디";
    private static readonly string skillNameString = "스킬 이름";
    private static readonly string conditionTypeString = "스킬 발동 조건";
    private static readonly string conditionValueString = "스킬 발동 수치";
    private static readonly string effectTypeString = "스킬 효과";
    private static readonly string effectValueString = "스킬 효과 값";
    private static readonly string effectDurationString = "효과 지속시간";
    private static readonly string skillTargetString = "스킬 효과 타겟";
    private static readonly string skillIconString = "스킬 아이콘";
    private static readonly string skillDescString = "스킬 설명";

    // 스킬 생성 변수들
    private int skillId = 0;
    private string skillName = string.Empty;
    private int conditionType = 0;
    private int conditionValue = 0;
    private int effectType = 0;
    private string effectValue = string.Empty;
    private int effectDuration = 0;
    private int skillTarget = 0;
    private string skillIcon = string.Empty;
    private string skillDesc = string.Empty;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 인스펙터 UI를 그립니다.

        if (GUILayout.Button(skillCreateButtonLabel))
        {
            skillCreateButtonShowDropdown = !skillCreateButtonShowDropdown;

            skillCreateButtonLabel = skillCreateButtonShowDropdown ? "캐릭터 스킬 생성 취소" : "캐릭터 스킬 생성";

            InitializeSkillVariables();

        }

        // 캐릭터 스킬 생성 눌렀을 시
        if (skillCreateButtonShowDropdown)
        {
            skillId = EditorGUILayout.IntField(skillIdString, skillId);
            skillName = EditorGUILayout.TextField(skillNameString, skillName);

            conditionType = EditorGUILayout.Popup(conditionTypeString, conditionType, ConditionTypeStrings);
            conditionValue = EditorGUILayout.IntField(conditionValueString, conditionValue);

            effectType = EditorGUILayout.Popup(effectTypeString, effectType, EffectTypeStrings);
            effectValue = EditorGUILayout.TextField(effectValueString, effectValue);
            effectDuration = EditorGUILayout.Popup(effectDurationString, effectDuration, EffectDurationStrings);

            skillTarget = EditorGUILayout.Popup(skillTargetString, skillTarget, SkillTargetStrings);
            skillIcon = EditorGUILayout.TextField(skillIconString, skillIcon);
            skillDesc = EditorGUILayout.TextField(skillDescString, skillDesc);

            if (GUILayout.Button("저장"))
            {
                CharacterSkillData newSkill = new CharacterSkillData
                {
                    SkillID = skillId,
                    SkillName = skillName,
                    ConditionType = conditionType,
                    ConditionValue = conditionValue,
                    EffectType = effectType + 1,
                    EffectValue = effectValue,
                    EffectDuration = effectDuration + 1,
                    Target = skillTarget + 1,
                    Skill_Icon = skillIcon,
                    Desc = skillDesc,
                };

                CharacterSkillTable skillTable = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
                if (skillTable != null)
                {
                    skillTable.AddOrUpdateSkill(newSkill , "CharacterSkillData.csv");
                }
            }
        }
    }
    private void InitializeSkillVariables()
    {
        skillId = 0;
        skillName = string.Empty;
        conditionType = 0;
        conditionValue = 0;
        effectType = 0;
        effectValue = string.Empty;
        effectDuration = 0;
        skillTarget = 0;
        skillIcon = string.Empty;
        skillDesc = string.Empty;
    }
}