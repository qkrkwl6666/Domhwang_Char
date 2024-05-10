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
    private int PositionSpacing = 3;

    private GameObject monster;

    // Todo : 이곳에서 테이블 가져와서 확률에 따라 몬스터 생성
    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
        monster = GameObject.FindWithTag("Monster");

        CharacterAnimationEvent.CharacterRunEvent += CharacterRun;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RoundCharacterSpawn());
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

    IEnumerator RoundCharacterSpawn()
    {
        removeCharacter.Clear();
        spawnXPosition = -10f;

        // 첫 라운드
        BattleSetCharacters(0 , 3);

        SetIdlePosition();

        yield return new WaitForSeconds(10f);

        // 2 라운드

    }

    public void BattleSetCharacters(int startIndex = 0, int endIndex = 5)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            characterList[i].GetComponent<CharacterControll>().runPercent = characterList[i].GetComponent<CharacterInfo>().Run;
            battleCharacter.Add(characterList[i]);
        }

        for (int i = 0; i < battleCharacter.Count; i++)
        {
            battleCharacter[i].SetActive(true);
            battleCharacter[i].transform.position = new Vector3(spawnXPosition, 0f, 0f);
            spawnXPosition -= 2f;
        }

        CharactersRun();

    }

    public void CharacterRun(CharacterControll characterControll)
    {
        characterControll.RunMode();
        if (characterControll.isRun)
        { 
            battleCharacter.Remove(characterControll.gameObject);
        }

    }

    public void CharactersRun()
    {
        removeCharacter.Clear();

        for (int i = 0; i < battleCharacter.Count; i++)
        {
            var characterMove = battleCharacter[i].GetComponent<CharacterControll>();
            characterMove.RunMode();
            if (characterMove.isRun) { removeCharacter.Add(battleCharacter[i]); }
        }

        for (int i = 0; i < removeCharacter.Count; i++)
        {
            battleCharacter.Remove(removeCharacter[i]);
        }
    }

    public void SetIdlePosition()
    {
        PositionSpacing = 3;

        for (int i = 0; i < battleCharacter.Count; i++)
        {
            var characterController = battleCharacter[i].GetComponent<CharacterControll>();

            characterController.StopPosition = monster.transform.position - new Vector3(PositionSpacing, 0,0);

            PositionSpacing += 1;
        }
    }
}
