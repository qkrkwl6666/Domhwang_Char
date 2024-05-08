using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using static CharacterTable;

public class SkillTable : DataTable
{
    public class SkillData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public string Desc { get; set; }

    }

    private Dictionary<string, SkillData> skillTable = new();

    public SkillData Get(string id)
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
            var records = csvReader.GetRecords<SkillData>();

            foreach (var record in records)
            {
                skillTable.Add(record.Id.ToString(), record);
            }
        }

    }
}
