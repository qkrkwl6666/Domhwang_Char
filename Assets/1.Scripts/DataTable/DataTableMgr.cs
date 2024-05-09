using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableMgr : Singleton<DataTableMgr>
{
    private Dictionary<string, DataTable> tables = new ();

    private void Awake()
    {
        DataTable characterTable = new CharacterTable();
        characterTable.Load("CharacterData");

        DataTable skillTable = new SkillTable();
        skillTable.Load("SkillData");

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load("MonsterData");

        tables.Add("Character", characterTable);
        tables.Add("Skill", skillTable);
        tables.Add("Monster", monsterTable);
    }


    private void Start()
    {

    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }
}
