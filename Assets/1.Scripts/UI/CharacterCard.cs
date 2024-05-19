using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour
{
    private CharacterData characterData;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnCardButton);
    }

    public void SetData(CharacterData characterData)
    {
        this.characterData = characterData;
    }

    public void OnCardButton()
    {
        GetComponentInParent<NewCharacter>().SetSelectCharacter(characterData);
        GetComponentInParent<NewCharacter>().OpenUIFormation();
    }
}
