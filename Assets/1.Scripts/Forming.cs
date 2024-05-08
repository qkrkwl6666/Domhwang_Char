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

    // PlayerInfo UI 
    public Image selectedCharacter;
    public TMPro.TextMeshProUGUI characterName;
    public TMPro.TextMeshProUGUI attack;
    public TMPro.TextMeshProUGUI run;
    public TMPro.TextMeshProUGUI skill;
    public TMPro.TextMeshProUGUI rare;

    // ���� ���� ���� ĳ���� ����Ʈ
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // �� ������ ĳ���� ����Ʈ
    public List<GameObject> selectCharacterList { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        // ���̺� ������ �ʱ� ĳ���� ����
        playerCharacterList = GameManager.instance.playerCharacterList;

       // foreach(GameObject character in playerCharacterList)
       // { 
       //     var go = Instantiate(UICharacterPrefabs, content.transform);
       //     go.name = character.name;
       //     var characterInfo = character.GetComponent<CharacterInfo>();
       //     characterInfo.characterData.Instance_Id = character.GetInstanceID();
       //     go.GetComponent<Image>().sprite = characterInfo.characterImage;
       //     go.GetComponent<ChatacterSlot>().characterData = characterInfo.characterData;
       //     go.GetComponent<ChatacterSlot>().characterImage = characterInfo.characterImage;
       // }
            

        
    }
    private void Start()
    {
        ChatacterSlot.OnCharacterUIInfo += UICharacterInfo;
    }

    public void UICharacterInfo(CharacterData characterData, Sprite img)
    {
        selectedCharacter.sprite = img;
        characterName.text = characterData.Name;
        attack.text = characterData.Atk.ToString();
        run.text = characterData.Run.ToString();
        skill.text = characterData.Skill_Id.ToString();
        rare.text = characterData.Tier;
    }

}
