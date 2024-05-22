using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableMgr : Singleton<DataTableMgr>
{
    private Dictionary<string, DataTable> tables = new ();
    private void Awake()
    {
        DataTable effectTable = new EffectTable();
        effectTable.Load("EffectsData");

        DataTable characterskillTable = new CharacterSkillTable();
        characterskillTable.Load("CharacterSkillData");

        DataTable characterTable = new CharacterTable();
        characterTable.Load("CharacterData_Designer");

        DataTable monsterTable = new MonsterTable();
        monsterTable.Load("MonsterData");

        tables.Add("Effect", effectTable);
        tables.Add("Character", characterTable);
        tables.Add("CharacterSkill", characterskillTable);
        tables.Add("Monster", monsterTable);

    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }
}
