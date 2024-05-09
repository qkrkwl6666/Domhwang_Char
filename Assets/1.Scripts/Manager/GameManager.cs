using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    public static readonly string pathData = "ScriptableObject/CharacterInitialData/";
    public static readonly string pathList = "Characters/";

    // 내가 보유 중인 캐릭터들
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // 편성 선택한 캐릭터 리스트
    public List<GameObject> formationCharacterList = new List<GameObject>();

    private void Awake()
    {
       
    }

    void Start()
    {
        if(SaveLoadSystem.Load() == null)
        {
            // 세이브 데이터가 없다면 기본 캐릭터 지급
            var go = DataTableMgr.instance.Get<CharacterTable>("Character");
            foreach (var c in go.characterTable)
            {
                var character = Instantiate(Resources.Load<GameObject>("Characters/" + c.Value.Id));
                var info = character.AddComponent<CharacterInfo>();
                info.SetCharacterData(c.Value);
                info.creationTime = System.DateTime.Now;
                info.InstanceId = Animator.StringToHash(info.creationTime.Ticks.ToString());

                character.SetActive(false);
                playerCharacterList.Add(character);
            }
        }
        else // 세이브 데이터 기반 캐릭터 생성
        {
            foreach(CharacterInfo go in SaveLoadSystem.CurrentData.characterDataList)
            {
                var character = Instantiate(Resources.Load<GameObject>("Characters/" + go.Id));
                var info = character.AddComponent<CharacterInfo>();
                info.SetCharacterInfo(go);

                character.SetActive(false);
                playerCharacterList.Add(character);
            }
            
        }
        

        // 세이브 데이터가 있다면 캐릭터 Load

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
    }
}
