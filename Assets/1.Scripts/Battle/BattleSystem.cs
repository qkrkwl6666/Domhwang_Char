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

    // ���� �� ĳ����
    public List<List<GameObject>> roundsCharacters = new List<List<GameObject>>();

    private float spawnXPosition = -10f;
    private int PositionSpacing = 3;

    public int Round { get; private set; } = 3;
    private int eventCount = 0;

    private GameObject monster;

    // Todo : �̰����� ���̺� �����ͼ� Ȯ���� ���� ���� ����
    private void Awake()
    {
        characterList = GameManager.Instance.formationCharacterList;
        monster = GameObject.FindWithTag("Monster");

        InitializeRoundCharacters(Round);

        CharacterControll.OnCharacterControll += EventCount;
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
    // Todo : ĳ���� �������� ��Ȱ��ȭ �� �߰��� ���Ͱ� ������ ����â
    // ���Ͱ� ���� �ʾ������ ĳ���Ϳ��� ���� 
    IEnumerator CharactersBattleSystem()
    {
        int currentRound = 1;

        while (currentRound <= Round)
        {
            removeCharacter.Clear();

            // ���庰 ĳ���� ����  
            BattleSetCharacters(currentRound);

            SetIdlePosition(currentRound);

            yield return StartCoroutine(WaitForCharactersIdle(currentRound));

            Debug.Log("���� ���� ĳ���� ����");
            RoundCharactersAttack(currentRound);

            currentRound++;
        }

        yield break;
    }

    IEnumerator WaitForCharactersIdle(int Round)
    {
        eventCount = 0;

        while (true)
        {
            var roundCharacters = roundsCharacters[Round - 1];

            var ccc = characterList[characterList.Count - 1].GetComponent<CharacterControll>();

            if (Round == 3 && ccc.attackEndRun)
            {
                yield return new WaitForSeconds(10f);
                break;
            }

            if(roundCharacters.Count == 0)
            {
                yield return new WaitForSeconds(6f);
                break;
            }

            if(eventCount == roundCharacters.Count)
            {
                eventCount = 0;
                break;
            }

            yield return null;
        }

        yield break;
    }

    public void EventCount()
    {
        ++eventCount;
        Debug.Log(eventCount);
    }

    public void InitializeRoundCharacters(int round = 3)
    {
        if (characterList == null) return;

        // 1���� ĳ����

        List<GameObject> round1 = new List<GameObject>();
        List<GameObject> round2 = new List<GameObject>();
        List<GameObject> round3 = new List<GameObject>();
        
        for (int i = 0; i < 3; i++)
        {
            round1.Add(characterList[i]);
        }

        roundsCharacters.Add(round1);

        // 2���� ĳ����
        for (int i = 3; i < 5; i++)
        {
            round2.Add(characterList[i]);
        }
        roundsCharacters.Add(round2);

        // 3���� ĳ����
        for(int i = 5; i <= 5; i++)
        {
            round3.Add(characterList[i]);
        }

        roundsCharacters.Add(round3);
    }

    public void RoundCharactersAttack(int Round)
    {
        removeCharacter.Clear();

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

                // ���� ������ ���� ����ĥ�� �ٽ� ���
                characterController.RunMode(false);
                if (characterController.attackEndRun)
                {
                    removeCharacter.Add(roundCharacters[i]);
                }
            }

            foreach (var character in removeCharacter)
            {
                roundCharacters.Remove(character);
            }

            removeCharacter.Clear();
            currentRound++;
        }

    }

    public void BattleSetCharacters(int currentRound)
    {
        if (currentRound > Round) return;

        spawnXPosition = -10f;
        // 1 -> 0
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

            // ���� ������ ���� ����ĥ�� �ٽ� ���
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
