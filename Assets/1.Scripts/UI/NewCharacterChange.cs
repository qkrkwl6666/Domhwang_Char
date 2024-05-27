using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewCharacterChange : MonoBehaviour
{
    public Transform characterUIContent;

    public NewCharacterInfo currentCharacterInfo;
    public NewCharacterInfo changeCharacterInfo;

    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> characterUIPrefabs = new List<GameObject>();

    public CharacterData selectCharacterData;

    public Button changeButton;
    public Button cencelButton;

    private void Awake()
    {
        changeButton.onClick.AddListener(OnChangeButtonClick);
        cencelButton.onClick.AddListener(OnCencelButtonClick);
    }

    private void OnEnable()
    {
        changeButton.interactable = false;
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
            go.GetComponent<CharacterChangeSlot>().SetCharacterSlot(character.GetComponent<CharacterInfo>());
        }
    }


    private void OnDisable()
    {
        selectCharacterData = null;

        foreach (Transform t in characterUIContent)
        {
            Destroy(t.gameObject);
        }
    }

    public void OnChangeButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        // 현재 플레이어 리스트 에서 제거

        GameManager.Instance.PlayerCharacterList.Remove(currentCharacterInfo.characterInfo.gameObject);

        for(int i = 0; i < GameManager.Instance.formationCharacterList.Count; i++)
        {
            if(currentCharacterInfo.characterInfo == GameManager.Instance.formationCharacterList[i])
            {
                GameManager.Instance.formationCharacterList[i] = null;
                break;
            }
        }

        Destroy(currentCharacterInfo.characterInfo.gameObject);

        // 카드 캐릭터 생성 후 플레이어 리스트에 넣기

        GameManager.Instance.CreateCharacter(changeCharacterInfo.characterData);

        GameManager.Instance.TryCount = 3;
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.MAIN);
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));
    }

    public void OnCencelButtonClick()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.CencelClip);
        // Todo : 데이터 테이블 연동
        GameManager.Instance.TryCount = 3;
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.MAIN);
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));
    }
}
