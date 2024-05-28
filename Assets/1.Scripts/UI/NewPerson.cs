using System.Collections.Generic;
using UnityEngine;

public class NewPerson : MonoBehaviour
{
    public Transform content;
    public GameObject cardPrefab;
    public readonly int newCharacterCount = 3;
    public List<CharacterData> charactersData = new List<CharacterData>();
    public System.Random random = new System.Random();
    public NewCharacterChange characterChange;

    public GameObject cardUI;

    private CharacterData selectCharacterData;

    private void OnEnable()
    {
        cardUI.SetActive(true);

        charactersData.Clear();

        foreach (Transform t in content.transform)
        {
            Destroy(t.gameObject);
        }

        // 등급별 캐릭터 소환
        for (int i = 0; i < newCharacterCount; i++)
        {
            charactersData.Add(GameManager.Instance.GetRandomCharacter((CharacterTier)i));
        }

        // 섞기
        Shuffle(charactersData);

        for (int i = 0; i < newCharacterCount; i++)
        {
            var card = Instantiate(cardPrefab, content);
            card.GetComponent<NewCharacterCard>().SetData(charactersData[i]);
            card.GetComponent<NewCharacterCard>().CardAwake();
        }
    }

    public void Shuffle<T>(IList<T> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }

    public void SetSelectCharacter(CharacterData data)
    {
        selectCharacterData = data;
    }

    public void OpenUIFormation()
    {
        characterChange.changeCharacterInfo.SetCharacterDataUI(selectCharacterData);
        cardUI.SetActive(false);

        UIManager.Instance.OpenUI(Page.CHARACTERCHANGE);
    }
}
