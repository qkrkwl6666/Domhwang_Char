using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleSystem : MonoBehaviour
{
    public List<GameObject> battleCharacter = new List<GameObject>();

    private List<GameObject> removeCharacter = new List<GameObject>();
    private List<GameObject> characterList;
    private float spawnXPosition = -10f;

    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
    }

    // Start is called before the first frame update
    void Start()
    {
        RoundCharacterSpawn(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Main");
            UIManager.Instance.OpenUI(Page.TITLE);
        }

    }

    private void RoundCharacterSpawn(int index)
    {
        removeCharacter.Clear();
        spawnXPosition = -10f;

        // 첫 라운드
        for (int i = 0; i < 3; i ++)
        {
            characterList[i].GetComponent<CharacterMove>().runPercent = characterList[i].GetComponent<CharacterInfo>().Run;
            battleCharacter.Add(characterList[i]);
        }

        for(int i = 0; i < battleCharacter.Count; i++)
        {
            battleCharacter[i].SetActive(true);
            battleCharacter[i].transform.position = new Vector3(spawnXPosition, 0f , 0f);
            spawnXPosition -= 2f;

            var characterMove = battleCharacter[i].GetComponent<CharacterMove>();
            characterMove.RunMode();
            if (characterMove.isRun) { removeCharacter.Add(battleCharacter[i]); }
        }

        for(int i = 0; i < removeCharacter.Count; i++)
        {
            battleCharacter.Remove(removeCharacter[i]);
        }

        


    }
}
