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
        foreach(Transform t in selectCharacterUI)
        {
            uiSelectCharacterList.Add(t.gameObject);
        }

        gameStartButton.interactable = false;
        gameStartButton.onClick.AddListener(OnClickGameStart);
    }   

    private void OnEnable()
    {
        GameManager.Instance.formationCharacterList.Clear();

        // Todo : 나중에 오브젝트풀로 관리
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

    }

    private void Start()
    {
        CharacterSlot.OnCharacterUIInfo += UICharacterInfo;
        CharacterSlot.OnCharacterUISelect += OnCharacterSelect;
    }

    public void UICharacterInfo(CharacterInfo characterInfo)
    {
        selectedCharacter.sprite = characterInfo.characterImage;
        characterName.text = characterInfo.Name;
        attack.text = characterInfo.Atk.ToString();
        run.text = characterInfo.Run.ToString();
        skill.text = characterInfo.Skill_Id.ToString();
        rare.text = characterInfo.Tier;
    }

    public void OnCharacterSelect(CharacterInfo characterInfo , CharacterSlot characterSlot)
    {
        int index = GameManager.Instance.formationCharacterList.Count;
        if (index > 5) return;

        Debug.Log(index);

        GameManager.Instance.formationCharacterList.Add(characterInfo.gameObject);
        var uiImg = uiSelectCharacterList[index].GetComponent<Image>();
        uiImg.sprite = characterInfo.characterImage;
        characterSlot.gameObject.SetActive(false);

        if(index == 5) gameStartButton.interactable = true;
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

}
