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

    // 내가 보유 중인 캐릭터 리스트
    public List<GameObject> playerCharacterList { get; private set; } = new List<GameObject>();

    // 편성 선택한 캐릭터 리스트
    public List<GameObject> selectCharacterList { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        // 세이브 없을때 초기 캐릭터 생성
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
