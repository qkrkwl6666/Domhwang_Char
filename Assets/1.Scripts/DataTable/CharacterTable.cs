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

    public List<CharacterData> GetAllCharacterDatas()
    {
        List<CharacterData> allCharacterDatas = new List<CharacterData>();

        foreach (var character in characterTable.Values)
        {
            allCharacterDatas.Add(character);
        }

        return allCharacterDatas;
    }

    public List<List<CharacterData>> GetTierCharacterDatasList()
    {
        List<List<CharacterData>> tierCharacterDatasList = new List<List<CharacterData>>();

        for(int i = 0; i < (int)CharacterTier.COUNT; i++)
        {
            tierCharacterDatasList.Add(new List<CharacterData>());
        }

        foreach (var character in characterTable.Values)
        {
            switch (character.Tier)
            {
                case "normal":
                    tierCharacterDatasList[(int)CharacterTier.NORMAL].Add(character);
                    break;
                case "rare":
                    tierCharacterDatasList[(int)CharacterTier.RARE].Add(character);
                    break;
                case "epic":
                    tierCharacterDatasList[(int)CharacterTier.EPIC].Add(character);
                    break;
            }
        }

        return tierCharacterDatasList;
    }
}
