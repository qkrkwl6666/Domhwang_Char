using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Runtime;
public class CharacterTable : DataTable
{
    public Dictionary<string, CharacterData> characterTable {get; private set; } = new Dictionary<string, CharacterData>();

    public CharacterData Get(string id)
    {
        if (!characterTable.ContainsKey(id)) return default;

        return characterTable[id];
    }
    public override void Load(string Path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(string.Format(FormatPath, Path));

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        }))
        {
            var records = csvReader.GetRecords<CharacterData>();

            foreach (var record in records)
            {
                characterTable.Add(record.Id.ToString(), record);
            }
        }
        
    }
}
