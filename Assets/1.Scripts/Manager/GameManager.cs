using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : Singleton<GameManager>
{
    public static readonly string pathData = "ScriptableObject/CharacterInitialData/";
    public static readonly string pathList = "Characters/";

    public readonly int MAX_FORMATION_SIZE = 8;
    public readonly int MAX_STAGE = 12;
    public Canvas canvas { get; private set; }
    public int TryCount { get; set; } = 3;

    // �÷��̾� ��������
    public int CurrentStage { get; private set; } = 0;
    // ���� 
    public MonsterData MonsterData { get; private set; }

    public bool gameRestart = false;

    // ��ü ĳ���� ������ ����Ʈ
    public List<CharacterData> AllCharacterDataList { get; private set; } = new List<CharacterData>();

    // ��޺� ĳ���� ������ ����Ʈ
    public List<List<CharacterData>> TierCharacterDatasList { get; private set; } = new List<List<CharacterData>>();

    // ���� ���� ���� ĳ���͵�
    public List<GameObject> PlayerCharacterList { get; private set; } = new List<GameObject>();

    // �� ������ ĳ���� ����Ʈ
    public List<GameObject> formationCharacterList = new List<GameObject>();

    // ������ ����Ʈ
    public List<GameObject> LevelUpCharacterList { get; set; } = new List<GameObject>();

    // Attack ��ƼŬ ����Ʈ
    public List<GameObject> AtkParticleSystemList { get; set; } = new List<GameObject>();

    // ����� �κ�
    public AudioSource AudioSource { get; private set; }
    public AudioClip OkClip { get; private set; }
    public AudioClip CencelClip { get; private set; }
    public AudioClip LoseClip { get; private set; }
    public AudioClip VictoryClip { get; private set; }

    public AudioSource BackgroundAudioSource { get; private set; }


    private void Awake()
    {
        // ĳ���� ������ ��������
        var characterDatas = DataTableMgr.Instance.Get<CharacterTable>("Character");

        AllCharacterDataList = characterDatas.GetAllCharacterDatas();

        TierCharacterDatasList = characterDatas.GetTierCharacterDatasList();

        InitialGameStart();

        canvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        SceneManager.sceneLoaded += GameManagerAwake;

        AudioSource = gameObject.AddComponent<AudioSource>();
        AudioSource.volume = 0.1f;

        AudioClipLoad();

        BackgroundAudioSource = gameObject.AddComponent<AudioSource>();
        BackgroundAudioSource.volume = 0.05f;
        BackgroundAudioSource.loop = true;

        BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/MainMenu"));

    }   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData1 saveData1 = new SaveData1();
            List<CharacterInfo> list = new List<CharacterInfo>();

            foreach(var character in PlayerCharacterList)
            {
                list.Add(character.GetComponent<CharacterInfo>());
            }
            saveData1.characterDataList = list;
            SaveLoadSystem.Save( -1, saveData1);

            Debug.Log("Save");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryCount--;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // Load
            SaveData1 save = SaveLoadSystem.Load() as SaveData1;
            Debug.Log("Load");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StageClear();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach(var character in formationCharacterList)
            {
                character.GetComponent<CharacterInfo>().BattleAttack = 1000;
            }
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

    public void GameManagerAwake(Scene scene, LoadSceneMode mode)
    {
        if(TryCount != 0 && scene.name == "Main")
        {
            Save();
        }
        if(scene.name == "Main")
        {
            CharacterAnimationEvent.MonsterDamageEvent = null;
            MonsterData = null;
            formationCharacterList.Clear();
            LevelUpCharacterList.Clear();
            CharactersCCEnable(true);

            foreach (var character in PlayerCharacterList)
            {
                var cc = character.GetComponent<CharacterControll>();
                var cl = character.GetComponent<CharacterInfo>();
                var ce = character.GetComponent<CharacterEffect>();

                cl.BattleAttack = cl.Atk;
                cc.CharacterAwake();
                ce.EffectAwake();
                character.SetActive(false);
            }
        }
    }

    public void GameWin()
    {
        Debug.Log("GameWin");

        AudioSource.Stop();
        AudioSource.PlayOneShot(VictoryClip);

        BackgroundAudioSource.Stop();
        BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/Forming"));

        StageClear();

        foreach(var character in LevelUpCharacterList)
        {
            character.GetComponent<CharacterInfo>().LevelUp();
        }

        UIManager.Instance.OpenUI(Page.LEVELUP);
    }

    public void GameLose()
    {
        TryCount--;
        GameObject.FindWithTag("BackgroundBGM").GetComponent<AudioSource>().Stop();

        if (TryCount == 0)
        {
            Debug.Log("���� �ʱ�ȭ");
            gameRestart = true;
            AudioSource.Stop();
            AudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/GameOver"));
        }
        else
        {
            AudioSource.Stop();
            AudioSource.PlayOneShot(LoseClip);
        }

        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.LOSE);
    }

    public void GameRestart()
    {
        foreach (var atkPrticle in AtkParticleSystemList)
        {
            atkPrticle.SetActive(true);
            Destroy(atkPrticle);
        }
        AtkParticleSystemList.Clear();

        // ���� ����� �� UI ���� 
        foreach (var character in PlayerCharacterList)
        {
            character.SetActive(true);
            Destroy(character);
        }

        formationCharacterList.Clear();
        PlayerCharacterList.Clear();
        LevelUpCharacterList.Clear();

        InitialCharacterCreate();

        TryCount = 3;
        CurrentStage = 0;
        gameRestart = false;
    }

    public void CharactersCCEnable(bool isEnable)
    {
        if (PlayerCharacterList == null) return;

        foreach(var gameObject in PlayerCharacterList)
        {
            if (gameObject == null) continue;

            var cc = gameObject.GetComponent<CharacterControll>();
            cc.enabled = isEnable;
        }
    }

    public void GameSetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public CharacterData GetRandomCharacter(CharacterTier tier)
    {
        var chList = TierCharacterDatasList[(int)tier];
        int count = chList.Count;
        int randomIndex = UnityEngine.Random.Range(0 , count);

        return TierCharacterDatasList[(int)tier][randomIndex];
    }
    public GameObject CreateCharacter(CharacterData characterData)
    {
        GameObject resourcesData = Resources.Load<GameObject>("Characters/" + characterData.Id);
        if (resourcesData == null) return null;

        var character = Instantiate(resourcesData);
        var info = character.AddComponent<CharacterInfo>();
        character.GetComponentInChildren<CharacterAnimationEvent>().characterInfo = info;

        info.SetCharacterData(characterData);
        character.GetComponent<CharacterEffect>().EffectInitialiAwake();
        info.creationTime = System.DateTime.Now;
        info.InstanceId = Animator.StringToHash(info.creationTime.Ticks.ToString());
        character.SetActive(false);
        DontDestroyOnLoad(character);
        PlayerCharacterList.Add(character);

        return character;
    }

    public void Save()
    {
        SaveData1 saveData1 = new SaveData1();
        List<CharacterInfo> list = new List<CharacterInfo>();

        saveData1.currentStage = CurrentStage;
        saveData1.tryCount = TryCount;

        foreach (var character in PlayerCharacterList)
        {
            list.Add(character.GetComponent<CharacterInfo>());
        }
        saveData1.characterDataList = list;
        SaveLoadSystem.Save(-1, saveData1);

        Debug.Log("Save");
    }

    public void Load()
    {
        CurrentStage = SaveLoadSystem.CurrentData.currentStage;
        TryCount = SaveLoadSystem.CurrentData.tryCount;

        foreach (CharacterInfo go in SaveLoadSystem.CurrentData.characterDataList)
        {
            var character = Instantiate(Resources.Load<GameObject>("Characters/" + go.Id));
            var info = character.AddComponent<CharacterInfo>();
            character.GetComponentInChildren<CharacterAnimationEvent>().characterInfo = info;
            info.SetCharacterInfo(go);
            character.GetComponent<CharacterEffect>().EffectInitialiAwake();

            DontDestroyOnLoad(character);
            character.SetActive(false);
            PlayerCharacterList.Add(character);
        }
    }

    public void InitialGameStart()
    {
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
                if (i == MAX_FORMATION_SIZE - 1) // ���� �̱�
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

                CreateCharacter(characterData);
            }

            Save();
        }// ���̺� �����Ͱ� �ִٸ� ĳ���� Load
        else if (PlayerCharacterList.Count == 0)// ���̺� ������ ��� ĳ���� ����
        {
            Load();
        }
    }

    public void InitialCharacterCreate()
    {
        int normalListCount = TierCharacterDatasList[(int)CharacterTier.NORMAL].Count;
        int rarelListCount = TierCharacterDatasList[(int)CharacterTier.RARE].Count;
        int randomIndex;
        int id;
        CharacterData characterData;
        // ���̺� �����Ͱ� ���ٸ� �⺻ ĳ���� ���� �Ϲ� 7��, ���� 1�� ����
        for (int i = 0; i < MAX_FORMATION_SIZE; i++) // 7�� ���� �̱�
        {
            if (i == MAX_FORMATION_SIZE - 1) // ���� �̱�
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

            CreateCharacter(characterData);
        }

        Save();
    }

    public void AudioClipLoad()
    {
        OkClip = Resources.Load<AudioClip>("Sound/OK");
        CencelClip = Resources.Load<AudioClip>("Sound/Cancel");
        LoseClip = Resources.Load<AudioClip>("Sound/Lose");
        VictoryClip = Resources.Load<AudioClip>("Sound/Victory");
}
}
