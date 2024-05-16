using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : Singleton<GameManager>
{
    public static readonly string pathData = "ScriptableObject/CharacterInitialData/";
    public static readonly string pathList = "Characters/";

    // 플레이어 스테이지
    public int CurrentStage { get; private set; } = 0;
    // 몬스터 
    public MonsterData MonsterData { get; private set; }

    // 전체 캐릭터 데이터 리스트
    public List<CharacterData> AllCharacterDataList { get; private set; } = new List<CharacterData>();

    // 등급별 캐릭터 데이터 리스트
    public List<List<CharacterData>> TierCharacterDatasList { get; private set; } = new List<List<CharacterData>>();

    // 내가 보유 중인 캐릭터들
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // 편성 선택한 캐릭터 리스트
    public List<GameObject> formationCharacterList = new List<GameObject>();

    private void Awake()
    {
        // 캐릭터 데이터 가져오기
        var characterDatas = DataTableMgr.Instance.Get<CharacterTable>("Character");

        AllCharacterDataList = characterDatas.GetAllCharacterDatas();

        TierCharacterDatasList = characterDatas.GetTierCharacterDatasList();

        if (SaveLoadSystem.Load() == null)
        {
            // 세이브 데이터가 없다면 기본 캐릭터 지급
            foreach (var c in characterDatas.characterTable)
            {
                GameObject resourcesData = Resources.Load<GameObject>("Characters/" + c.Value.Id);
                if (resourcesData == null) continue;
                var character = Instantiate(resourcesData);
                var info = character.AddComponent<CharacterInfo>();
                info.SetCharacterData(c.Value);
                info.creationTime = System.DateTime.Now;
                info.InstanceId = Animator.StringToHash(info.creationTime.Ticks.ToString());
                DontDestroyOnLoad(character);
                character.SetActive(false);
                playerCharacterList.Add(character);
            }
        }// 세이브 데이터가 있다면 캐릭터 Load
        else if (playerCharacterList.Count == 0)// 세이브 데이터 기반 캐릭터 생성
        {
            foreach (CharacterInfo go in SaveLoadSystem.CurrentData.characterDataList)
            {
                var character = Instantiate(Resources.Load<GameObject>("Characters/" + go.Id));
                var info = character.AddComponent<CharacterInfo>();
                info.SetCharacterInfo(go);
                DontDestroyOnLoad(character);
                character.SetActive(false);
                playerCharacterList.Add(character);
            }
        }


    }   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData1 saveData1 = new SaveData1();
            List<CharacterInfo> list = new List<CharacterInfo>();

            foreach(var character in playerCharacterList)
            {
                list.Add(character.GetComponent<CharacterInfo>());
            }
            saveData1.characterDataList = list;
            SaveLoadSystem.Save( -1, saveData1);

            Debug.Log("Save");
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            // Load
            SaveData1 save = SaveLoadSystem.Load() as SaveData1;
            Debug.Log("Load");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StageClear();
        }
    }

    public void CreateMonster()
    {
        MonsterTable monsterTable = DataTableMgr.Instance.Get<MonsterTable>("Monster");

        // 현재 스테이지 몬스터만 가져오기
        List<MonsterData> currentStageMonsterDatas = new List<MonsterData>();
        int totalWeight = 0;

        foreach (KeyValuePair<string, MonsterData> data in monsterTable.monsterTable)
        {
            if(CurrentStage == data.Value.Stage - 1)
            {
                currentStageMonsterDatas.Add(data.Value);
                totalWeight += data.Value.Enc;
            }
        }

        foreach(var data in currentStageMonsterDatas)
        {
            data.weight = (float)data.Enc / totalWeight;
        }

        // 오름차순 정렬 
        currentStageMonsterDatas.Sort((a,b) => a.weight.CompareTo(b.weight));

        float dcc = 0f;
        float randomMonster = UnityEngine.Random.Range(0f, 1f);

        foreach (var data in currentStageMonsterDatas)
        {
            dcc += data.weight;

            // 몬스터 세팅
            if (randomMonster <= dcc)
            {
                MonsterData = data;
                break;
            }
        }
    }

    public void StageClear()
    {
        ++CurrentStage;
    }

    public void GameManagerAwake()
    {
        MonsterData = null;
        formationCharacterList.Clear();

        foreach(var character in playerCharacterList)
        {
            var cc = character.GetComponent<CharacterControll>();
            cc.CharacterAwake();
            character.SetActive(false);
        }
    }

    public void GameWin()
    {

    }

    public void GameLose()
    {
        UIManager.Instance.OpenUI(Page.LOSE);
    }
}
