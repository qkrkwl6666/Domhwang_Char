using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChange : MonoBehaviour
{
    public Transform content;
    public GameObject characterSlotPrefab;
    public CardUIInfo SelectedCharacter;

    private void OnEnable()
    {
        foreach(var character in GameManager.Instance.playerCharacterList)
        {
            var go = Instantiate(characterSlotPrefab, content);
            var slot = go.GetComponent<FormationSlot>();
            slot.SetData(character.GetComponent<CharacterInfo>());
        }
    }
}
