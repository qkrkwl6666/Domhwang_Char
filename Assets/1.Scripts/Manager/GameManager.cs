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

    // ���� ���� ���� ĳ���͵�
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // �� ������ ĳ���� ����Ʈ
    public List<GameObject> formationCharacterList = new List<GameObject>();

    private void Awake()
    {
       
    }

    void Start()
    {
        if(SaveLoadSystem.Load() == null)
        {
            // ���̺� �����Ͱ� ���ٸ� �⺻ ĳ���� ����
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
        else // ���̺� ������ ��� ĳ���� ����
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
        

        // ���̺� �����Ͱ� �ִٸ� ĳ���� Load

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
