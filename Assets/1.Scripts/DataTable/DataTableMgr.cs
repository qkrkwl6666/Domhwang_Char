using System.Collections.Generic;

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

        DataTable stageTable = new StageTable();
        stageTable.Load("StageData");

        tables.Add("Effect", effectTable);
        tables.Add("Character", characterTable);
        tables.Add("CharacterSkill", characterskillTable);
        tables.Add("Monster", monsterTable);
        tables.Add("Stage", stageTable);
    }

    public T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id)) return default;

        return tables[id] as T;
    }
}
