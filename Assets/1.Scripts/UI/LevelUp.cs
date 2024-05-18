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

            uiPrefab.characterImage.sprite = character.characterImage;
            uiPrefab.levelText.text = $"{character.Level.ToString()}";
            uiPrefab.attackText.text = $"{character.Atk.ToString()} +{character.Atk_Up.ToString()}";
            uiPrefab.runText.text = $"{character.Run.ToString()} -{character.Run_Up.ToString()}";
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
        GameManager.Instance.GameManagerAwake();
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.STAGE);
    }
}
