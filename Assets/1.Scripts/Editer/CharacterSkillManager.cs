using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataTableMgr))]
public class CharacterSkillManager : Editor
{
    private string[] ConditionTypeStrings = new string[] 
    { 
        "���� ����", "���� ü���� ConditionValue���� �϶�", "�ܷ� ���簡 ConditionValue�̻��� �� ",
        "���� ĳ���Ͱ� �ܷ��� ������ ��", "�Բ� ���ݿ� ������ �ο��� �ִٸ�",
        "Ȧ�� ���ݿ� ���� �� ��", "ConditionValue ���� ���� �϶�", "ConditionValue ���忡��",
        "ConditionValue ���� ���� ���� ���¿��� ������ ��", "�ش� ĳ���Ͱ� ���ݿ� ���� �� ��",
    };

    private string[] EffectTypeStrings = new string[]
    {
        "���ݷ� �� EffectValue ��ŭ ����", "������ ����" , "���� ���� �ܷ�",
        "Ʈ���� ī��Ʈ �Ҹ���� ����", "�ݵ�� ���� �ϸ� �ݵ�� �ܷ� ���� ����",
        "���� �ο� �� ���� ���� ���ݷ� ����",
    };

    private string[] EffectDurationStrings = new string[]
    {
        "���� ����",
    };

    private string[] SkillTargetStrings = new string[]
    {
        "�ڽ��� ĳ����","�ܷ� ����","��� �ο�","���� ������ ���� �ο�","�ش� ������ ĳ����"
    };

    // ��ư ����
    private string skillCreateButtonLabel = "ĳ���� ��ų ����";
    private bool skillCreateButtonShowDropdown = false;

    private static readonly string skillIdString = "��ų ���̵�";
    private static readonly string skillNameString = "��ų �̸�";
    private static readonly string conditionTypeString = "��ų �ߵ� ����";
    private static readonly string conditionValueString = "��ų �ߵ� ��ġ";
    private static readonly string effectTypeString = "��ų ȿ��";
    private static readonly string effectValueString = "��ų ȿ�� ��";
    private static readonly string effectDurationString = "ȿ�� ���ӽð�";
    private static readonly string skillTargetString = "��ų ȿ�� Ÿ��";
    private static readonly string skillIconString = "��ų ������";
    private static readonly string skillDescString = "��ų ����";

    // ��ų ���� ������
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
        base.OnInspectorGUI(); // �⺻ �ν����� UI�� �׸��ϴ�.

        if (GUILayout.Button(skillCreateButtonLabel))
        {
            skillCreateButtonShowDropdown = !skillCreateButtonShowDropdown;

            skillCreateButtonLabel = skillCreateButtonShowDropdown ? "ĳ���� ��ų ���� ���" : "ĳ���� ��ų ����";

            InitializeSkillVariables();

        }

        // ĳ���� ��ų ���� ������ ��
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

            if (GUILayout.Button("����"))
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