using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class RunStringData
{
    public int Id { get; set; }
    public string RunString { get; set; }
}

public class RunStringTable : DataTable
{
    public Dictionary<string, RunStringData> runStringTable { get; private set; } = new();

    public RunStringData Get(string id)
    {
        if (!runStringTable.ContainsKey(id)) return default;

        return runStringTable[id];
    }

    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<RunStringData>();

            foreach (var record in records)
            {
                runStringTable.Add(record.Id.ToString(), record);
            }
        }
    }
}
