using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class EffectData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Pos { get; set; }
    public int ConditionType { get; set; }
    public int Tier { get; set; }
    public string File { get; set; }
}

public class EffectTable : DataTable
{
    public Dictionary<string, EffectData> effectTable { get; private set; } = new();

    public EffectData Get(string id)
    {
        if (!effectTable.ContainsKey(id)) return default;

        return effectTable[id];
    }

    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<EffectData>();

            foreach (var record in records)
            {
                effectTable.Add(record.Id.ToString(), record);
            }
        }

    }
}
