using CsvHelper;
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
    public string Desc { get; set; }
}
public class CharacterSkillTable : DataTable
{

    private Dictionary<string, CharacterSkillData> skillTable = new();

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
}
