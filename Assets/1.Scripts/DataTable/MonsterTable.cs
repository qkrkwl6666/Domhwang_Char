using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MonsterTable : DataTable
{
    public class MonsterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stage { get; set; }
        public int Hp { get; set; }
        public string Tier { get; set; }
        public int Enc { get; set; }
        public int Feature_Id { get; set; }
        public string round { get; set; }
        public string reduced_dmg { get; set; }
        public string heal { get; set; }

    }

    private Dictionary<string, MonsterData> monsterTable = new();

    public MonsterData Get(string id)
    {
        if (!monsterTable.ContainsKey(id)) return default;

        return monsterTable[id];
    }
    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<MonsterData>();

            foreach (var record in records)
            {
                monsterTable.Add(record.Id.ToString(), record);
            }
        }

    }
}
