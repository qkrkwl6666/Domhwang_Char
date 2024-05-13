using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using static UnityEditor.ShaderData;


public class BattleSystem : MonoBehaviour
{
    public List<List<GameObject>> battleCharacter { get; private set; } = new List<List<GameObject>>();

    private List<GameObject> removeCharacters = new List<GameObject>();
    private List<GameObject> characterList;
    private GameObject removeCharacter = null;

    // 라운드 별 캐릭터
    public List<List<GameObject>> roundsCharacters { get; private set; } = new List<List<GameObject>>();

    // 전투 중인 캐릭터
    public List<GameObject> playingCharacters { get; private set; } = new List<GameObject>();

    // 잔류 병사 캐릭터
    public List<GameObject> remainingCharacters { get; private set; } = new List<GameObject>();

    // 대기 잔류 병사 다음 라운드에 잔류 병사에 추가할 리스트
    public List<GameObject> StandRemainingCharacters { get; private set; } = new List<GameObject>();

    private Vector3 lastPosition = Vector3.zero;
    private float spawnXPosition = -10f;
    private int PositionSpacing = 3;

    public int Round { get; private set; } = 3;

    private GameObject monster;

    // Todo : 이곳에서 테이블 가져와서 확률에 따라 몬스터 생성
    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
        monster = GameObject.FindWithTag("Monster");

        InitializeRoundCharacters(Round);

        CharacterControll.OnCharacterControll += IdleToEvent;
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
        int currentRound = 1;

        while (currentRound <= Round)
        {
            removeCharacters.Clear();

            // 전 라운드 잔류 병사 추가
            InsertStandRemainingCharacters();

            // 라운드별 캐릭터 스폰  
            BattleSetCharacters(currentRound);

            SetIdlePosition(currentRound);

            // 현재 라운드 공격이 끝났는지 대기
            yield return StartCoroutine(WaitForCharactersIdle());

            // Todo : 라운드에 나오는 캐릭터가 모두 도망가거나 1Run 1AttackEndRound || 2 Run 일때 시간 조절 해야함
            //yield return RunTimeCheck(currentRound);
            
            Debug.Log("현재 라운드 공격 끝남");

            // 남은 병사가 있으면 남은 병사 공격
            RemainingCharactersAttack();
            
            yield return StartCoroutine(WaitForCharactersIdle());

            currentRound++;
        }

        yield break;
    }

    IEnumerator RunTimeCheck(int currentRound)
    {
        if (roundsCharacters[currentRound - 1].Count == 0)
        {
            bool isPass = false;
            while (!isPass)
            {
                for (int i = 0; i < battleCharacter[currentRound - 1].Count; i++)
                {
                    if(i == battleCharacter[currentRound - 1].Count - 1 && !battleCharacter[currentRound - 1][i].activeSelf)
                    {
                        isPass = true;
                    }
                    else break;
                }
            }
        }

        yield break;
    }


    IEnumerator WaitForCharactersIdle()
    {
        while (true)
        {
            if(playingCharacters.Count == 0) yield break;

            yield return null;
        }
    }

    public void IdleToEvent(GameObject gameObject)
    {
        if (gameObject == null) return;

        playingCharacters.Remove(gameObject);
    }

    public void InitializeRoundCharacters(int round = 3)
    {
        if (characterList == null) return;

        // 1라운드 캐릭터

        List<GameObject> round1 = new List<GameObject>();
        List<GameObject> round2 = new List<GameObject>();
        List<GameObject> round3 = new List<GameObject>();

        battleCharacter.Add(new List<GameObject>());

        for (int i = 0; i < 3; i++)
        {
            round1.Add(characterList[i]);
            battleCharacter[0].Add(characterList[i]);
        }

        battleCharacter.Add(new List<GameObject>());
        roundsCharacters.Add(round1);

        // 2라운드 캐릭터
        for (int i = 3; i < 5; i++)
        {
            round2.Add(characterList[i]);
            battleCharacter[1].Add(characterList[i]);
        }

        roundsCharacters.Add(round2);
        battleCharacter.Add(new List<GameObject>());
        // 3라운드 캐릭터
        for (int i = 5; i <= 5; i++)
        {
            round3.Add(characterList[i]);
            battleCharacter[2].Add(characterList[i]);
        }

        roundsCharacters.Add(round3);
    }

    public void RoundCharactersAttack(int Round)
    {
        removeCharacters.Clear();

        int currentRound = 1;

        while (currentRound < Round)
        {
            var roundCharacters = roundsCharacters[currentRound - 1];

            if(roundCharacters.Count == 0)
            {
                ++currentRound;
                continue;
            }

            for (int i = 0; i < roundCharacters.Count; i++)
            {
                var characterController = roundCharacters[i].GetComponent<CharacterControll>();
                characterController.ChangeStatus(CharacterControll.Status.Move);
                characterController.AnimationMove();

                // 공격 끝나고 난후 도망칠지 다시 계산
                characterController.RunMode(false);
                if (characterController.attackEndRun)
                {
                    removeCharacters.Add(roundCharacters[i]);
                }
            }

            foreach (var character in removeCharacters)
            {
                roundCharacters.Remove(character);
            }

            removeCharacters.Clear();
            currentRound++;
        }
    }

    public void InsertStandRemainingCharacters()
    {
        foreach(var character in StandRemainingCharacters)
        {
            remainingCharacters.Add(character);
        }

        StandRemainingCharacters.Clear();
    }

    public void BattleSetCharacters(int currentRound)
    {
        if (currentRound > Round) return;

        spawnXPosition = -10f;

        --currentRound;

        var currentRoundCharacters = roundsCharacters[currentRound];

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            currentRoundCharacters[i].GetComponent<CharacterControll>().runPercent = currentRoundCharacters[i].GetComponent<CharacterInfo>().Run;
        }

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            currentRoundCharacters[i].SetActive(true);
            currentRoundCharacters[i].transform.position = new Vector3(spawnXPosition, 0f, 0f);
            spawnXPosition -= 2f;
        }

        CharactersRun(currentRoundCharacters);

    }

    public void CharactersRun(List<GameObject> charactersList = null)
    {
        removeCharacters.Clear();

        foreach (var character in charactersList)
        {
            var characterControll = character.GetComponent<CharacterControll>();

            characterControll.RunMode(true);

            if (characterControll.isRun) 
            {
                removeCharacters.Add(character);
                continue;
            }

            // 공격 끝나고 난후 도망칠지 다시 계산
            characterControll.RunMode(false);
            if (characterControll.attackEndRun) 
            {
                removeCharacters.Add(character);
                continue;
            }

            // 잔류 병사 추가 && 플레이 중인 병사 추가
            if(!characterControll.isRun && !characterControll.attackEndRun)
            {
                StandRemainingCharacters.Add(character);
                playingCharacters.Add(character);
            }
        }

        foreach(var removeCharacter in removeCharacters)
        {
            charactersList.Remove(removeCharacter);
        }
    }

    public void RemainingCharactersAttack()
    {
        removeCharacters.Clear();

        if (remainingCharacters.Count <= 0) return;

        foreach (var character in remainingCharacters)
        {
            var characterControll = character.GetComponent<CharacterControll>();

            characterControll.AttackMode();

            if (characterControll.attackEndRun) removeCharacters.Add(character);
            else playingCharacters.Add(character);
        }

        foreach(var removeCharacter in removeCharacters)
        {
            remainingCharacters.Remove(removeCharacter);
        }
    }

    public void PlayingListRemove()
    {
        if(removeCharacter == null) return;

        playingCharacters.Remove(removeCharacter);

        removeCharacter = null;
    }

    public void SetIdlePosition(int Round)
    {
        PositionSpacing = 3;

        int currentRound = 1;

        while (currentRound <= Round)
        {
            var roundCharacters = roundsCharacters[currentRound - 1];

            if (roundCharacters.Count == 0)
            {
                currentRound++;
                continue;
            }

            for (int i = 0; i < roundCharacters.Count; i++)
            {
                var characterController = roundCharacters[i].GetComponent<CharacterControll>();

                characterController.StopPosition = monster.transform.position - new Vector3(PositionSpacing, 0, 0);

                PositionSpacing += 1;
            }

            currentRound++;

        }
    }
}
