using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StageData
{
    public int Stage { get; set; }
    public int C_Level { get; set; }
    public string Background_Id { get; set; }
}

public class StageTable : DataTable
{
    public Dictionary<string, StageData> stageTable { get; private set; } = new();

    public StageData Get(string id)
    {
        if (!stageTable.ContainsKey(id)) return default;

        return stageTable[id];
    }

    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<StageData>();

            foreach (var record in records)
            {
                stageTable.Add(record.Stage.ToString(), record);
            }
        }

    }
}
