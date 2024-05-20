using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    public Transform Content;
    public GameObject LevelUpUIPrefab;

    private void OnEnable()
    {
        var levelUpCharacterList = GameManager.Instance.LevelUpCharacterList;

        foreach(var character in levelUpCharacterList)
        {
            var prefab = Instantiate(LevelUpUIPrefab, Content);
            var uiPrefab = prefab.GetComponent<LevelUpUIPrefab>();

            //uiPrefab.characterImage.sprite = character.characterImage;
            var go = Resources.Load("CharacterModel/" + character.Id.ToString()) as GameObject;
            var model = Instantiate(go, uiPrefab.characterModel.transform);
            model.transform.localScale = new Vector3(100f, 100f, 100f);
            model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);

            uiPrefab.levelText.text = $"{character.Level}";
            uiPrefab.attackText.text = $"{character.Atk} +{character.Atk_Up}";
            uiPrefab.runText.text = $"{character.Run} -{character.Run_Up}";
        }   
    }

    private void OnDisable()
    {
        foreach(Transform transform in Content)
        {
            Destroy(transform.gameObject);
        }
    }

    public void OnNextButton()
    {
        //GameManager.Instance.GameManagerAwake();
        //SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.NEWHERO);
    }
}
