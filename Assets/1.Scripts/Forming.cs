using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

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

    // SelectCharacter UI
    public Transform selectCharacterUI;

    // UI 캐릭터 보유중인 리스트
    public List<GameObject> uiCharacterList { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        foreach(Transform t in selectCharacterUI)
        {
            uiCharacterList.Add(t.gameObject);
        }
    }   

    private void OnEnable()
    {
        uiCharacterList.Clear();
        GameManager.instance.formationCharacterList.Clear();

        for (int i = 0; i < uiCharacterList.Count; i++)
        {
            Destroy(uiCharacterList[i]);
            uiCharacterList.RemoveAt(i);
        }

        foreach (GameObject character in GameManager.instance.playerCharacterList)
        {
            var uiCharacter = Instantiate(UICharacterPrefabs, content.transform);
            CharacterInfo characterInfo = character.GetComponent<CharacterInfo>();
            uiCharacter.GetComponent<Image>().sprite = characterInfo.characterImage;
            uiCharacter.GetComponent<ChatacterSlot>().characterInfo = characterInfo;
            uiCharacter.GetComponent<ChatacterSlot>().characterImage = characterInfo.characterImage;
            uiCharacterList.Add(uiCharacter);
        }
    }

    private void Start()
    {
        ChatacterSlot.OnCharacterUIInfo += UICharacterInfo;
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

    public void OnCharacterSelect(in CharacterInfo characterInfo)
    {
        if (GameManager.instance.formationCharacterList.Count > 6) return;

        for(int i = 0; i < uiCharacterList.Count; i++)
        {

        }
    }

}
