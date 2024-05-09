using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleSystem : MonoBehaviour
{
    private List<GameObject> battleCharacter = new List<GameObject>();

    private List<GameObject> characterList;
    private float spawnXPosition = -10f;

    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
    }

    // Start is called before the first frame update
    void Start()
    {
        RoundCharacterSpawn();
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

    private void RoundCharacterSpawn()
    {
        spawnXPosition = -10f;

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
        }
    }
}
