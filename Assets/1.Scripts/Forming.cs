using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Forming : MonoBehaviour
{
    public GameObject UICharacterPrefabs;
    public Transform content;

    // CharacterInfo UI 
    public Image selectedCharacter;
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

    // UI ĳ���� ���� ����Ʈ
    [field:SerializeField] public List<GameObject> uiSelectCharacterList { get; private set; } = new List<GameObject>();

    // UI ĳ���� �������� ����Ʈ
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

        // Todo : ���߿� ������ƮǮ�� ����
        for (int i = 0; i < uiCharacterList.Count; i++)
        {
            Destroy(uiCharacterList[i]);
            uiCharacterList.RemoveAt(i);
        }

        foreach (GameObject character in GameManager.Instance.playerCharacterList)
        {
            var uiCharacter = Instantiate(UICharacterPrefabs, content.transform);
            CharacterInfo characterInfo = character.GetComponent<CharacterInfo>();
            uiCharacter.GetComponent<Image>().sprite = characterInfo.characterImage;
            uiCharacter.GetComponent<CharacterSlot>().characterInfo = characterInfo;
            uiCharacter.GetComponent<CharacterSlot>().characterImage = characterInfo.characterImage;
            uiCharacterList.Add(uiCharacter);
        }

        foreach(GameObject uiCharacterSelect in uiSelectCharacterList)
        {
            uiCharacterSelect.GetComponent<Image>().sprite = default;
        }

        // �� �������� �´� ���� �̱�
        GameManager.Instance.CreateMonster();

        // MonsterUI
        monsterName.text = GameManager.Instance.MonsterData.Name;
        monsterHp.text = "Hp : " + GameManager.Instance.MonsterData.Hp.ToString();

        // ���� ���� ��ư ��Ȱ��ȭ
        gameStartButton.interactable = false;
    }

    private void Start()
    {
        CharacterSlot.OnCharacterUIInfo += UICharacterInfo;
        CharacterSlot.OnCharacterUISelect += OnCharacterSelect;
    }

    private void Update()
    {
        if(MultiTouchManager.Instance.LongTap == true)
        {
            Debug.Log("LongTap");
        }
    }

    public void UICharacterInfo(CharacterInfo characterInfo)
    {
        if (MultiTouchManager.Instance.LongTap == false) return;

        selectedCharacter.sprite = characterInfo.characterImage;
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

        var uiImg = uiSelectCharacterList[index].GetComponent<Image>();
        uiSelectCharacterList[index].GetComponent<CharacterSelectSlot>().characterSlot = characterSlot; 
        uiImg.sprite = characterInfo.characterImage;

        gameStartButton.interactable = GameStartCheck();
    }

    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Battle");
        UIManager.Instance.AllClose();
    }

    public void DefaultSetting()
    {
        selectedCharacter.sprite = default;
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
