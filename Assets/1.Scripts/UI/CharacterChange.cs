using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterChange : MonoBehaviour
{
    public Transform content;
    public GameObject characterSlotPrefab;
    public CardUIInfo SelectedCharacter;

    public Button ChangeButton;
    public Button CencelButton;

    public CardUIInfo removeCharacterCard;
    private CharacterInfo removeCharacterInfo;

    private void Awake()
    {
        ChangeButton.onClick.AddListener(OnChangeButtonClick);
        CencelButton.onClick.AddListener(OnCencelButtonClick);
    }

    private void OnEnable()
    {
        foreach (var character in GameManager.Instance.PlayerCharacterList)
        {
            var go = Instantiate(characterSlotPrefab, content);
            var slot = go.GetComponent<FormationSlot>();
            slot.SetData(character.GetComponent<CharacterInfo>());
        }
    }

    private void OnDisable()
    {
        ChangeButton.interactable = false;
        removeCharacterInfo = null;

        foreach (Transform transform in content.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    public void RemoveCardInfo(CharacterInfo characterInfo)
    {
        removeCharacterInfo = characterInfo;
        removeCharacterCard.SetData(removeCharacterInfo.ConvertCharacterData());
    }

    public void OnChangeButtonClick()
    {
        
        // 현재 플레이어 리스트 에서 제거
        GameManager.Instance.PlayerCharacterList.Remove(removeCharacterInfo.gameObject);

        // 카드 캐릭터 생성 후 플레이어 리스트에 넣기
        GameManager.Instance.CreateCharacter(SelectedCharacter.CharacterData);

        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.STAGE);

    }

    public void OnCencelButtonClick()
    {
        UIManager.Instance.OpenUI(Page.STAGE);
    }
}
