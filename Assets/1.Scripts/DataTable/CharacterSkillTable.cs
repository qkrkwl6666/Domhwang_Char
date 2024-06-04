using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterSkillData
{
    public int SkillID { get; set; }
    public string SkillName { get; set; }
    public int ConditionType { get; set; }
    public int ConditionValue { get; set; }
    public int EffectType { get; set; }
    public string EffectValue { get; set; }
    public int EffectDuration { get; set; }
    public int Target { get; set; }
    public string Skill_Icon { get; set; }
    public string Desc { get; set; }
}
public class CharacterSkillTable : DataTable
{

    public Dictionary<string, CharacterSkillData> skillTable {  get; private set; } = new Dictionary<string, CharacterSkillData>();

    public CharacterSkillData Get(string id)
    {
        if (!skillTable.ContainsKey(id)) return default;

        return skillTable[id];
    }
    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<CharacterSkillData>();

            foreach (var record in records)
            {
                skillTable.Add(record.SkillID.ToString(), record);
            }
        }
    }

    public void Save(string Path)
    {

        using (var writer = new StreamWriter(string.Format(FormatPath2, Path), false, new System.Text.UTF8Encoding(true)))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
            };

            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(skillTable.Values);
            }
        }
    }

    public void AddOrUpdateSkill(CharacterSkillData skillData, string Path)
    {
        skillTable[skillData.SkillID.ToString()] = skillData; // 기존 ID가 있으면 업데이트, 없으면 추가
        Save(Path); // 변경된 데이터를 파일에 저장
    }
}
