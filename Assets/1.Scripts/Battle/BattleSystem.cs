using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using static UnityEditor.ShaderData;


public class BattleSystem : MonoBehaviour
{
    public List<GameObject> battleCharacter = new List<GameObject>();
    private List<GameObject> removeCharacter = new List<GameObject>();
    private List<GameObject> characterList;

    public List<GameObject> round1Characters;
    public List<GameObject> round2Characters;
    public GameObject round3Characters;

    private float spawnXPosition = -10f;
    private int PositionSpacing = 3;

    private GameObject monster;

    // Todo : 이곳에서 테이블 가져와서 확률에 따라 몬스터 생성
    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
        monster = GameObject.FindWithTag("Monster");

        InitializeRoundCharacters();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CharactersBattleSystem());
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
    // Todo : 캐릭터 도망가면 비활성화 및 중간에 몬스터가 죽으면 보상창
    // 몬스터가 죽지 않았을경우 캐릭터에게 공격 
    IEnumerator CharactersBattleSystem()
    {
        removeCharacter.Clear();

        // 첫 라운드
        BattleSetCharacters(round1Characters);

        SetIdlePosition(round1Characters);

        // 1라운드 캐릭터 공격 끝나고 2라운드 시작 (연동 작업)
        yield return StartCoroutine(WaitForCharactersIdle(round1Characters));

        Debug.Log("2라운드 시작");

        // 2 라운드

        BattleSetCharacters(round2Characters);

        SetIdlePosition(round1Characters,round2Characters);

        // 2라운드 캐릭터 공격 끝나고 >> 1라운드 남은 캐릭터 공격 시작
        yield return StartCoroutine(WaitForCharactersIdle(round2Characters));

        // 1라운드 캐릭터 공격 시작
        Debug.Log("1라운드 캐릭터 공격 시작");

        RoundCharactersAttack(round1Characters);

        yield return StartCoroutine(WaitForCharactersIdle(round1Characters));

        // Todo : 캐릭터가 공격후 도망가면 Idle 위치 새로 고침

        // 3라운드 시작

        Debug.Log("3라운드 시작");

        BattleSetCharacters(ref round3Characters);

        SetIdlePosition(round1Characters, round2Characters, round3Characters);

        // 3라운드 캐릭터 공격이 끝나고 남은 1 2 라운드 캐릭터 공격

        yield return StartCoroutine(WaitForCharactersIdle(null, null, round3Characters));
        // Todo : 3라운드 캐릭터 도망가는거 연동 체크 해야함 << 현재 3라운드 캐릭터가 
        // 공격후 AttackEnd 에 걸렸을때 남은 캐릭터들이 바로 공격하는 현상 고쳐야함
        Debug.Log("3라운드 캐릭터 공격이 끝나고 남은 1 2 라운드 캐릭터 공격");

        RoundCharactersAttack(round1Characters);
        RoundCharactersAttack(round2Characters);

        // 공격이 다끝났는지 확인
        yield return StartCoroutine(WaitForCharactersIdle(round1Characters, round2Characters, round3Characters));

        Debug.Log("보스 공격 or 클리어");
    }

    IEnumerator WaitForCharactersIdle(List<GameObject> round1 = null, List<GameObject> round2 = null , GameObject round3 = null)
    {
        bool ready = false;

        // 1라운드 캐릭터
        while (!ready)
        {
            if (round1 == null || round1.Count == 0) break;

            for (int i = 0; i < round1.Count; i++)
            {
                var controll = round1[i].GetComponent<CharacterControll>();
                if (i == round1.Count - 1 && controll.status == CharacterControll.Status.Idle)
                {
                    ready = true;
                }
                else if (controll.status != CharacterControll.Status.Idle)
                {
                    break;
                }
            }

            yield return null;
        }

        ready = false;

        // 2라운드 캐릭터
        while (!ready)
        {
            if (round2 == null || round2.Count == 0) break;

            for (int i = 0; i < round2.Count; i++)
            {
                var controll = round2[i].GetComponent<CharacterControll>();
                if (i == round2.Count - 1 && controll.status == CharacterControll.Status.Idle)
                {
                    ready = true;
                }
                else if (controll.status != CharacterControll.Status.Idle)
                {
                    break;
                }
            }

            yield return null;
        }

        ready = false;

        // 3라운드 캐릭터
        while (!ready)
        {
            if (round3 == null) break;

            var controll = round3.GetComponent<CharacterControll>();

            if (controll.status == CharacterControll.Status.Idle) { ready = true; }

            yield return null;
        }

    }

    public void InitializeRoundCharacters()
    {
        if (characterList == null) return;

        // 1라운드 캐릭터

        for (int i = 0; i < 3; i++)
        {
            round1Characters.Add(characterList[i]);
        }

        // 2라운드 캐릭터

        for (int i = 3; i < 5; i++)
        {
            round2Characters.Add(characterList[i]);
        }

        round3Characters = characterList[characterList.Count - 1];
    }

    public void RoundCharactersAttack(List<GameObject> roundCharacters)
    {
        removeCharacter.Clear();

        for (int i = 0; i < roundCharacters.Count; i++)
        {
            var characterController = roundCharacters[i].GetComponent<CharacterControll>();
            characterController.ChangeStatus(CharacterControll.Status.Move);
            characterController.AnimationMove();

            // 공격 끝나고 난후 도망칠지 다시 계산
            characterController.RunMode(false);
            if (characterController.attackEndRun)
            {
                removeCharacter.Add(roundCharacters[i]);
            }
        }

        foreach(var character in removeCharacter)
        {
            roundCharacters.Remove(character);
        }

        removeCharacter.Clear();
    }

    public void BattleSetCharacters(List<GameObject> charactersList = null)
    {
        if (charactersList == null) return;

        spawnXPosition = -10f;

        for (int i = 0; i < charactersList.Count - 1; i++)
        {
            charactersList[i].GetComponent<CharacterControll>().runPercent = charactersList[i].GetComponent<CharacterInfo>().Run;
        }

        for (int i = 0; i < charactersList.Count; i++)
        {
            charactersList[i].SetActive(true);
            charactersList[i].transform.position = new Vector3(spawnXPosition, 0f, 0f);
            spawnXPosition -= 2f;
        }

        CharactersRun(charactersList);

    }

    public void BattleSetCharacters(ref GameObject character)
    {
        if (character == null) return;

        spawnXPosition = -10f;

        character.GetComponent<CharacterControll>().runPercent = character.GetComponent<CharacterInfo>().Run;

        character.SetActive(true);
        character.transform.position = new Vector3(spawnXPosition, 0f, 0f);
        spawnXPosition -= 2f;

        CharactersRun(ref character);
    }

    public void CharactersRun(List<GameObject> charactersList = null)
    {
        removeCharacter.Clear();

        foreach (var character in charactersList)
        {
            var characterControll = character.GetComponent<CharacterControll>();

            characterControll.RunMode(true);

            if (characterControll.isRun) 
            {
                removeCharacter.Add(character);
                continue;
            }

            // 공격 끝나고 난후 도망칠지 다시 계산
            characterControll.RunMode(false);
            if (characterControll.attackEndRun) 
            {
                removeCharacter.Add(character);
                continue;
            }
        }

        foreach(var removeCharacter in removeCharacter)
        {
            charactersList.Remove(removeCharacter);
        }
    }

    public void CharactersRun(ref GameObject character)
    {
        removeCharacter.Clear();

        if(character == null) return;

        var characterControll = character.GetComponent<CharacterControll>();

        characterControll.RunMode(true);

        if (characterControll.isRun)
        {
            character = null;
        }

        // 공격 끝나고 난후 도망칠지 다시 계산
        characterControll.RunMode(false);
        if (characterControll.attackEndRun)
        {
            character = null;
        }
    }

    public void SetIdlePosition(List<GameObject> roun1Characters = null, List<GameObject> roun2Characters = null, 
        GameObject roun3Characters = null)
    {
        PositionSpacing = 3;

        // 1라운드 캐릭터 위치
        if(roun1Characters != null)
        {
            for (int i = 0; i < roun1Characters.Count; i++)
            {
                var characterController = roun1Characters[i].GetComponent<CharacterControll>();

                characterController.StopPosition = monster.transform.position - new Vector3(PositionSpacing, 0, 0);

                PositionSpacing += 1;
            }
        }

        // 2라운드 캐릭터 위치
        if (roun2Characters != null)
        {
            for (int i = 0; i < roun2Characters.Count; i++)
            {
                var characterController = roun2Characters[i].GetComponent<CharacterControll>();

                characterController.StopPosition = monster.transform.position - new Vector3(PositionSpacing, 0, 0);

                PositionSpacing += 1;
            }
        }

        // 3라운드 위치
        if(roun3Characters != null)
        {
            var characterController = roun3Characters.GetComponent<CharacterControll>();

            characterController.StopPosition = monster.transform.position - new Vector3(PositionSpacing, 0, 0);
        }
    }
}
