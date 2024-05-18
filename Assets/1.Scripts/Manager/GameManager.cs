using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : Singleton<GameManager>
{
    public static readonly string pathData = "ScriptableObject/CharacterInitialData/";
    public static readonly string pathList = "Characters/";

    public readonly int MAX_FORMATION_SIZE = 8;

    public Canvas canvas { get; private set; }

    // �÷��̾� ��������
    public int CurrentStage { get; private set; } = 0;
    // ���� 
    public MonsterData MonsterData { get; private set; }

    // ��ü ĳ���� ������ ����Ʈ
    public List<CharacterData> AllCharacterDataList { get; private set; } = new List<CharacterData>();

    // ��޺� ĳ���� ������ ����Ʈ
    public List<List<CharacterData>> TierCharacterDatasList { get; private set; } = new List<List<CharacterData>>();

    // ���� ���� ���� ĳ���͵�
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // �� ������ ĳ���� ����Ʈ
    public List<GameObject> formationCharacterList = new List<GameObject>();

    // ������ ����Ʈ
    public List<CharacterInfo> LevelUpCharacterList { get; private set; } = new List<CharacterInfo>();

    private void Awake()
    {
        // ĳ���� ������ ��������
        var characterDatas = DataTableMgr.Instance.Get<CharacterTable>("Character");

        AllCharacterDataList = characterDatas.GetAllCharacterDatas();

        TierCharacterDatasList = characterDatas.GetTierCharacterDatasList();

        if (SaveLoadSystem.Load() == null)
        {
            int normalListCount = TierCharacterDatasList[(int)CharacterTier.NORMAL].Count;
            int rarelListCount = TierCharacterDatasList[(int)CharacterTier.RARE].Count;
            int randomIndex;
            int id;
            CharacterData characterData;
            // ���̺� �����Ͱ� ���ٸ� �⺻ ĳ���� ���� �Ϲ� 7��, ���� 1�� ����
            for (int i = 0; i < MAX_FORMATION_SIZE; i++) // 7�� ���� �̱�
            {
                if(i == MAX_FORMATION_SIZE - 1) // ���� �̱�
                {
                    randomIndex = UnityEngine.Random.Range(0, rarelListCount);
                    characterData = TierCharacterDatasList[(int)CharacterTier.RARE][randomIndex];
                    id = characterData.Id;

                }
                else
                {
                    randomIndex = UnityEngine.Random.Range(0, normalListCount);
                    characterData = TierCharacterDatasList[(int)CharacterTier.NORMAL][randomIndex];
                    id = characterData.Id;
                }

                GameObject resourcesData = Resources.Load<GameObject>("Characters/" + id);
                if (resourcesData == null) continue;

                var character = Instantiate(resourcesData);
                var info = character.AddComponent<CharacterInfo>();
                character.GetComponentInChildren<CharacterAnimationEvent>().characterInfo = info;

                info.SetCharacterData(characterData);
                info.creationTime = System.DateTime.Now;
                info.InstanceId = Animator.StringToHash(info.creationTime.Ticks.ToString());
                character.SetActive(false);
                DontDestroyOnLoad(character);
                playerCharacterList.Add(character);
            }
            
        }// ���̺� �����Ͱ� �ִٸ� ĳ���� Load
        else if (playerCharacterList.Count == 0)// ���̺� ������ ��� ĳ���� ����
        {
            foreach (CharacterInfo go in SaveLoadSystem.CurrentData.characterDataList)
            {
                var character = Instantiate(Resources.Load<GameObject>("Characters/" + go.Id));
                var info = character.AddComponent<CharacterInfo>();
                character.GetComponentInChildren<CharacterAnimationEvent>().characterInfo = info;
                info.SetCharacterInfo(go);
                DontDestroyOnLoad(character);
                character.SetActive(false);
                playerCharacterList.Add(character);
            }
        }

        canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
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

        // ���� �������� ���͸� ��������
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

        // �������� ���� 
        currentStageMonsterDatas.Sort((a,b) => a.weight.CompareTo(b.weight));

        float dcc = 0f;
        float randomMonster = UnityEngine.Random.Range(0f, 1f);

        foreach (var data in currentStageMonsterDatas)
        {
            dcc += data.weight;

            // ���� ����
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
        CharacterAnimationEvent.MonsterDamageEvent = null;
        MonsterData = null;
        formationCharacterList.Clear();
        LevelUpCharacterList.Clear();
        CharactersCCEnable(true);
        
        foreach (var character in playerCharacterList)
        {
            var cc = character.GetComponent<CharacterControll>();
            cc.CharacterAwake();
            character.SetActive(false);
        }
    }

    public void GameWin()
    {
        Debug.Log("GameWin");
        StageClear();

        foreach (var character in formationCharacterList)
        {
            var cc = character.GetComponent<CharacterControll>();
            if(!cc.isRun && !cc.attackEndRun)
            {
                var ci = character.GetComponent<CharacterInfo>();
                ci.LevelUp();
                LevelUpCharacterList.Add(ci);
            }
        }

        UIManager.Instance.OpenUI(Page.LEVELUP);
    }

    public void GameLose()
    {
        UIManager.Instance.OpenUI(Page.LOSE);
    }

    public void CharactersCCEnable(bool isEnable)
    {
        foreach(var gameObject in playerCharacterList)
        {
            var cc = gameObject.GetComponent<CharacterControll>();
            cc.enabled = isEnable;
        }
    }

    public void GameSetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

}
