using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterChange : MonoBehaviour
{
    public Transform characterUIContent;

    public NewCharacterInfo currentCharacterInfo;
    public NewCharacterInfo changeCharacterInfo;

    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> characterUIPrefabs = new List<GameObject>();

    private void OnEnable()
    {
        characters.Clear();

        foreach (GameObject character in GameManager.Instance.PlayerCharacterList)
        {
            var cc = character.GetComponent<CharacterInfo>();

            int index = 0;

            switch (cc.Tier)
            {
                case "normal":
                    index = 0;
                    break;
                case "rare":
                    index = 1;
                    break;
                case "epic":
                    index = 2;
                    break;
            }

            var go = Instantiate(characterUIPrefabs[index], characterUIContent);
            characters.Add(go);
            go.GetComponent<CharacterNewSlot>().SetCharacterSlot(character.GetComponent<CharacterInfo>());
        }
    }
}
