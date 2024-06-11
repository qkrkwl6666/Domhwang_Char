using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MonsterSkillData
{
    public int SkillID { get; set; }
    public string Desc { get; set; }
    public int ConditionType { get; set; }
    public int ConditionValue { get; set; }
    public int EffectType { get; set; }
    public string EffectValue { get; set; }
    public int Target { get; set; }
}
public class MonsterSkillTable : DataTable
{
    public Dictionary<string, MonsterSkillData> skillTable { get; private set; } = new Dictionary<string, MonsterSkillData>();

    public MonsterSkillData Get(string id)
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
            var records = csvReader.GetRecords<MonsterSkillData>();

            foreach (var record in records)
            {
                skillTable.Add(record.SkillID.ToString(), record);
            }
        }
    }

}
