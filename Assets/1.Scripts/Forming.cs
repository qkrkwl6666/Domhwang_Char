using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Forming : MonoBehaviour
{
    public GameObject UICharacterPrefabs;
    public Transform content;

    // CharacterInfo UI 
    public Transform selectedCharacter;
    public TMPro.TextMeshProUGUI characterName;
    public TMPro.TextMeshProUGUI attack;
    public TMPro.TextMeshProUGUI run;
    public TMPro.TextMeshProUGUI skill;
    public TMPro.TextMeshProUGUI rare;

    // MonsterUI
    public GameObject monsterUi;
    public Button monsterShowUiButton;
    public TMPro.TextMeshProUGUI monsterName;
    public TMPro.TextMeshProUGUI monsterHp;
    public Transform monsterContent;
    public TextMeshProUGUI monsterDesc;

    // characterInfoIO
    public GameObject infoDesc;
    public Button SkillInfoButton;
    public TextMeshProUGUI infoDescText;

    public CharacterInfo InfoCharacter;

    // GameStartUI
    public Button gameStartButton;

    // SelectCharacter UI
    public Transform selectCharacterUI;

    // UI 캐릭터 선택 리스트
    public List<GameObject> uiSelectCharacterList { get; private set; } = new List<GameObject>();

    // UI 캐릭터 보유중인 리스트
    public List<GameObject> uiCharacterList { get; private set; } = new List<GameObject>();

    private void Awake()
    {

        int i = 0;
        foreach(Transform t in selectCharacterUI)
        {
            GameObject go = t.gameObject;
            go.GetComponent<CharacterSelectSlot>().SlotIndex = i++;
            uiSelectCharacterList.Add(go);
        }

        gameStartButton.interactable = false;
        gameStartButton.onClick.AddListener(OnClickGameStart);
        monsterShowUiButton.onClick.AddListener(OnMonsterUiButtonClick);
        SkillInfoButton.onClick.AddListener(OnButtonSkillIconClick);

        SceneManager.sceneLoaded += FormingUiAwake;
    }   

    private void OnEnable()
    {
        GameManager.Instance.formationCharacterList.Clear();

        for (int i = 0; i < 6; i++)
        {
            GameManager.Instance.formationCharacterList.Add(null);
        }

        // Todo : 나중에 오브젝트풀로 관리
        for (int i = 0; i < uiCharacterList.Count; i++)
        {
            Destroy(uiCharacterList[i]);
        }

        uiCharacterList.Clear();

        foreach (GameObject character in GameManager.Instance.PlayerCharacterList)
        {
            var uiCharacter = Instantiate(UICharacterPrefabs, content.transform);
            CharacterInfo characterInfo = character.GetComponent<CharacterInfo>();
            var resource = Resources.Load("CharacterModel/" + characterInfo.Id) as GameObject;
            var model = Instantiate(resource, uiCharacter.transform);
            model.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3 (0, -50, 0);
            uiCharacter.GetComponent<CharacterSlot>().characterInfo = characterInfo;
            uiCharacter.GetComponent<CharacterSlot>().levelText.text = characterInfo.Level.ToString();
            //uiCharacter.GetComponent<CharacterSlot>().characterImage = characterInfo.characterImage;
            uiCharacterList.Add(uiCharacter);
        }

        //foreach(GameObject uiCharacterSelect in uiSelectCharacterList)
        //{
        //    uiCharacterSelect.GetComponent<Image>().sprite = default;
        //}

        // 각 스테이지 맞는 몬스터 뽑기
        GameManager.Instance.CreateMonster();

        // MonsterUI
        monsterName.text = GameManager.Instance.MonsterData.Name;
        monsterHp.text = "Hp : " + GameManager.Instance.MonsterData.Hp.ToString();
        monsterDesc.text = GameManager.Instance.MonsterData.Desc.ToString();

        // 몬스터 모델 생성 Todo Battle 씬 이동시 파괴
        var monster = Resources.Load<GameObject>("MonsterModel/" + GameManager.Instance.MonsterData.Id);
        var monsterModel = Instantiate(monster, monsterContent.transform);
        monsterModel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -50f, 0f);

        // 게임 시작 버튼 비활성화
        gameStartButton.interactable = false;


        // 캐릭터 Info Ui
        infoDesc.SetActive(false);

        foreach (Transform transform in selectedCharacter.transform)
        {
            Destroy(transform.gameObject);
        }

        characterName.text = "";
        attack.text = "";
        run.text = "";
        skill.text = "";
        rare.text = "";
        infoDescText.text = "";
    }

    private void Start()
    {
        CharacterSlot.OnCharacterUIInfo += UICharacterInfo;
        //CharacterSlot.OnCharacterUISelect += OnCharacterSelect;
    }

    private void Update()
    {

    }

    public void UICharacterInfo(CharacterInfo characterInfo, CharacterSlot characterSlot)
    {
        //if (MultiTouchManager.Instance.LongTap == false) return;

        if (MultiTouchManager.Instance.Tap == false) return;

        if (characterInfo == InfoCharacter)
        {
            OnCharacterSelect(characterInfo, characterSlot);
            InfoCharacter = null;

            foreach (Transform transform in selectedCharacter.transform)
            {
                Destroy(transform.gameObject);
            }

            characterName.text = "";
            attack.text = "";
            run.text = "";
            skill.text = "";
            rare.text = "";
            infoDescText.text = "";

            return;
        }

        InfoCharacter = characterInfo;

        foreach (Transform transform in selectedCharacter.transform)
        {
            Destroy(transform.gameObject);
        }

        //selectedCharacter.sprite = characterInfo.characterImage;

        var resource = Resources.Load("CharacterModel/" + characterInfo.Id) as GameObject;
        var model = Instantiate(resource, selectedCharacter.transform);
        model.transform.localScale = new Vector3(200f, 200f, 200f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -70, 0);
        characterName.text = characterInfo.Name;
        attack.text = characterInfo.Atk.ToString();
        run.text = characterInfo.Run.ToString();
        skill.text = characterInfo.Skill_Id.ToString();
        rare.text = characterInfo.Tier;
        if(characterInfo.Skill_Id == 0)
        {
            infoDescText.text = "이 친구는 무능력자 입니다";
        }
        else
        {
            var table = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill");
            var data = table.Get(characterInfo.Skill_Id.ToString());
            infoDescText.text = data.Desc;
        }

    }
        
    public void OnCharacterSelect(CharacterInfo characterInfo , CharacterSlot characterSlot)
    {
        //if (MultiTouchManager.Instance.Tap == false) return;

        List<GameObject> list = GameManager.Instance.formationCharacterList;
        int index = -1;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null) continue;

            list[i] = characterInfo.gameObject;
            index = i;
            break;
        }

        if (index == -1) return;

        characterSlot.gameObject.SetActive(false);
        
        var characterSlotGo = characterSlot.gameObject;
        //var characterModel = characterSlotGo.transform.GetChild(0);
        GameObject characterModel = null;
        foreach(Transform t in characterSlotGo.transform)
        {
            if(t.GetComponent<TextMeshProUGUI>() == null)
            {
                characterModel = t.gameObject;
                break;
            }
        }
        var uiSelectGo = uiSelectCharacterList[index];

        characterModel.transform.SetParent(uiSelectGo.transform);
        characterModel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -50f, 0f);

        uiSelectCharacterList[index].GetComponent<CharacterSelectSlot>().characterSlot = characterSlot; 

        gameStartButton.interactable = GameStartCheck();
    }

    public void OnClickGameStart()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        SceneManager.LoadScene("Battle");

        UIManager.Instance.AllClose();
        
    }

    public void DefaultSetting()
    {
        //selectedCharacter
        characterName.text = "Name";
        attack.text = "Attack";
        run.text = "Run";
        skill.text = "Skill";
        rare.text = "Rare";
    }

    private bool GameStartCheck()
    {
        var list = GameManager.Instance.formationCharacterList;

        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == null) return false;
        }

        if (monsterUi.activeSelf || infoDesc.activeSelf) return false;

        return true;
    }

    public void OnMonsterUiButtonClick()
    {
        monsterUi.SetActive(monsterUi.activeSelf ? false : true);

        if (monsterUi.activeSelf)
        {
            foreach(var item in uiSelectCharacterList)
            {
                item.SetActive(false);
                gameStartButton.interactable = false;
            }
        }
        else
        {
            foreach (var item in uiSelectCharacterList)
            {
                item.SetActive(true);
                gameStartButton.interactable = GameStartCheck();
            }
        }

    }
    public void FormingUiAwake(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Battle")
        {
            foreach(Transform t in monsterContent)
            {
                Destroy(t.gameObject);
            }
        }
    }

    public void OnButtonSkillIconClick()
    {
        //bool isActive = infoDesc.activeSelf ? false : true;
        if (infoDesc.activeSelf)
        {
            infoDesc.SetActive(false);
            gameStartButton.interactable = GameStartCheck();
        }
        else
        {
            infoDesc.SetActive(true);
            gameStartButton.interactable = false;
        }

       

        //infoDescText.text = 

    }

}
