using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Numerics;
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
    public TMPro.TextMeshProUGUI monsterName;
    public TMPro.TextMeshProUGUI monsterHp;

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

        // 게임 시작 버튼 비활성화
        gameStartButton.interactable = false;
    }

    private void Start()
    {
        CharacterSlot.OnCharacterUIInfo += UICharacterInfo;
        CharacterSlot.OnCharacterUISelect += OnCharacterSelect;
    }

    private void Update()
    {

    }

    public void UICharacterInfo(CharacterInfo characterInfo)
    {
        if (MultiTouchManager.Instance.LongTap == false) return;

        foreach(Transform transform in selectedCharacter.transform)
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
    }
        
    public void OnCharacterSelect(CharacterInfo characterInfo , CharacterSlot characterSlot)
    {
        if (MultiTouchManager.Instance.Tap == false) return;

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

        return true;
    }

}
