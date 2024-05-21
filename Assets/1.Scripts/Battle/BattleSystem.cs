using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BattleSystem : MonoBehaviour
{
    public TextMeshProUGUI roundTextUI;
    public List<List<GameObject>> battleCharacter { get; private set; } = new List<List<GameObject>>();

    private List<GameObject> removeCharacters = new List<GameObject>();
    private List<GameObject> characterList;

    // ���� �� ĳ����
    public List<List<GameObject>> roundsCharacters { get; private set; } = new List<List<GameObject>>();

    public List<GameObject> playingCharacters = new List<GameObject>();
    public List<GameObject> remainingCharacters { get; private set; } = new List<GameObject>();

    // ��� �ܷ� ���� ���� ���忡 �ܷ� ���翡 �߰��� ����Ʈ
    public List<GameObject> StandRemainingCharacters { get; private set; } = new List<GameObject>();

    private float spawnXPosition = -10f;
    private int PositionSpacing = 3;

    private List<Vector3> IdlePoint = new List<Vector3>();
    public int Round { get; private set; } = 3;
    public int CurrentRound { get; private set; } = 1;

    public bool RemainingAttack { get; private set; } = false;

    private GameObject monster;
    public MonsterInfo MonsterInfo { get; private set; }

    // Todo : �̰����� ���̺� �����ͼ� Ȯ���� ���� ���� ����
    private void Awake()
    {

    }

    private void OnEnable()
    {
        characterList = GameManager.Instance.formationCharacterList;

        var go = Resources.Load<GameObject>("Monsters/" + GameManager.Instance.MonsterData.Id.ToString());
        // ���� ����
        monster = Instantiate(go);
        MonsterInfo = monster.GetComponent<MonsterInfo>();

        InitializeRoundCharacters(Round);

        CharacterControll.OnCharacterControll += IdleToEvent;

        SetIdlePoint();
    }

    // Start is called before the first frame update    
    void Start()
    {
        StartCoroutine(CharactersBattleSystem());
    }

    // Todo : �߰��� ���Ͱ� ������ ����â ���Ͱ� ���� �ʾ������ ĳ���Ϳ��� ���� 
    IEnumerator CharactersBattleSystem()
    {
        while (CurrentRound <= Round)
        {
            RemainingAttack = false;
            MonsterInfo.CurrentRound = CurrentRound;

            BossSkills();
            ApplyInitializeSkill();

            roundTextUI.text = CurrentRound.ToString() + " ����";
            removeCharacters.Clear();

            // �� ���� �ܷ� ���� �߰�
            InsertStandRemainingCharacters();

            // ���庰 ĳ���� ����  
            BattleSetCharacters(CurrentRound);
            ApplyCharacterSkills(CurrentRound);

            SetIdlePosition(CurrentRound);

            // ���� ���� ������ �������� ���
            yield return StartCoroutine(WaitForCharactersIdle(true, CurrentRound));

            Stage6BossSkill();
            MonsterInfo.isIncreasedDamage = false;

            // ���� ���簡 ������ ���� ���� ����
            RemainingCharactersAttack();

            SetIdlePosition(CurrentRound);

            yield return StartCoroutine(WaitForCharactersIdle());

            CurrentRound++;
        }

        // �������� ������ 2���� ����
        yield return StartCoroutine(WaitFor2Sec());

        // ���� ����
        yield return StartCoroutine(MonsterAttack());

        // ĳ���Ͱ� ���ư�����

        foreach(var characterList in roundsCharacters)
        {
            foreach (var character in characterList)
            {
                var cc = character.GetComponent<CharacterControll>();
                cc.ChangeStatus(CharacterControll.Status.Fly);
            }
        }

        yield return new WaitForSeconds(3f);

        UIManager.Instance.OpenUI(Page.LOSE);

        //���ư��� 3�ʵ� Lose UI // ���׾����� �й�
        if (!monster.GetComponent<MonsterInfo>().isDead)
        {
            GameManager.Instance.GameLose();
        }

        yield break;
    }

    IEnumerator WaitFor2Sec()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator WaitForCharactersIdle(bool firstRound = false, int currentRound = -1)
    {
        if(playingCharacters.Count == 0 && !firstRound && currentRound == -1) { yield break; }
        else if (playingCharacters.Count == 0 && firstRound)
        {
            bool isPass = false;
        
            while (!isPass)
            {
                for (int i = 0; i < battleCharacter[currentRound - 1].Count; i++)
                {
                    var currentRoundCharacters = battleCharacter[currentRound - 1];
                    var character = currentRoundCharacters[i];

                    if (i == currentRoundCharacters.Count - 1 && !character.activeSelf)
                    {
                        yield break;
                    }
                    else if (character.activeSelf) break;
                }

                yield return null;
            }
        }
        else
        {
            while (true)
            {
                if(playingCharacters.Count == 0) yield break;

                yield return null;
            }
        }

        yield break;
    }

    public void IdleToEvent(GameObject gameObject)
    {
        if (gameObject == null) return;

        playingCharacters.Remove(gameObject);
    }

    public void InitializeRoundCharacters(int round = 3)
    {
        if (characterList == null) return;

        // 1���� ĳ����

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

        // 2���� ĳ����
        for (int i = 3; i < 5; i++)
        {
            round2.Add(characterList[i]);
            battleCharacter[1].Add(characterList[i]);
        }

        roundsCharacters.Add(round2);
        battleCharacter.Add(new List<GameObject>());
        // 3���� ĳ����
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

                // ���� ������ ���� ����ĥ�� �ٽ� ���
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

        var currentRoundCharacters = roundsCharacters[currentRound - 1];

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            var characterControll = currentRoundCharacters[i].GetComponent<CharacterControll>();
            characterControll.runPercent = currentRoundCharacters[i].GetComponent<CharacterInfo>().Run;
            characterControll.MonsterTransform = monster.transform;

            var animationEvent = currentRoundCharacters[i].GetComponentInChildren<CharacterAnimationEvent>();
            animationEvent.UpdateMonsterInfo();
        }

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            currentRoundCharacters[i].SetActive(true);
            currentRoundCharacters[i].transform.position = new Vector3(spawnXPosition, 0.5f, 0f);
            spawnXPosition -= 2f;
        }

        CharactersRun(currentRoundCharacters);

    }

    public void CharactersRun(List<GameObject> charactersList = null)
    {
        removeCharacters.Clear();
        var monsterInfo = monster.GetComponent<MonsterInfo>();

        int totalAttack = 0;
        foreach (var character in charactersList)
        {
            var characterControll = character.GetComponent<CharacterControll>();

            if(CurrentRound >= Round)
            {
                StandRemainingCharacters.Add(character);
                playingCharacters.Add(character);
                break;
            }

            characterControll.RunMode(true);

            if (characterControll.isRun) 
            {
                removeCharacters.Add(character);
                continue;
            }

            var ci = character.GetComponent<CharacterInfo>();

            totalAttack += ci.BattleAttack;

            if (monsterInfo.Hp - totalAttack <= 0) continue;

            characterControll.RunMode(false);
            if (characterControll.attackEndRun)
            {
                removeCharacters.Add(character);
                continue;
            }

            // �ܷ� ���� �߰� && �÷��� ���� ���� �߰�
            if (!characterControll.isRun && !characterControll.attackEndRun)
            {
                StandRemainingCharacters.Add(character);
                playingCharacters.Add(character);
            }
        }

        foreach (var removeCharacter in removeCharacters)
        {
            charactersList.Remove(removeCharacter);
        }

        removeCharacters.Clear();
    }

    public void RemainingCharactersAttack()
    {
        removeCharacters.Clear();

        if (remainingCharacters.Count <= 0) return;

        int totalAttack = 0;

        foreach (var character in remainingCharacters)
        {
            var characterControll = character.GetComponent<CharacterControll>();
            var ci = character.GetComponent<CharacterInfo>();

            totalAttack += ci.BattleAttack;

            characterControll.AttackMode();

            if (CurrentRound >= Round)
            {
                playingCharacters.Add(character);
                continue;
            }

            var monsterInfo = monster.GetComponent<MonsterInfo>();
            
            if (monsterInfo.Hp - totalAttack <= 0) continue;

            characterControll.RunMode(false);
            if (characterControll.attackEndRun) removeCharacters.Add(character);
            else playingCharacters.Add(character);
        }

        foreach(var removeCharacter in removeCharacters)
        {
            remainingCharacters.Remove(removeCharacter);

            foreach (var characterList in roundsCharacters)
            {
                characterList.Remove(removeCharacter);
            }
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

            Vector3 idlePosition = IdlePoint[currentRound - 1];

            for (int i = 0; i < roundCharacters.Count; i++)
            {
                var characterController = roundCharacters[i].GetComponent<CharacterControll>();

                characterController.StopPosition = idlePosition;

                idlePosition += new Vector3(-1f, 0f, 0f);
            }
            currentRound++;
        }
    }
                    
    public void SetIdlePoint()
    {
        // 1���� Idle ��ġ
        IdlePoint.Add(monster.transform.position + new Vector3(-3f , 1.0f, 0f));

        // 2���� Idle ��ġ
        IdlePoint.Add(monster.transform.position + new Vector3(-3f, -1f, 0f));

        // 3���� Idle ��ġ
        IdlePoint.Add(monster.transform.position + new Vector3(-3f , 0f, 0f));

    }

    IEnumerator MonsterAttack()
    {
        Animator monsterAnimator = monster.GetComponent<Animator>();
        monsterAnimator.SetTrigger("Attack");

        var monsterInfo = monster.GetComponent<MonsterInfo>();

        // ������ ������ ������ ���� ���
        while (true)
        {
            if (monsterInfo.MonsterAttackEnd) yield break;

            yield return null;
        }
    }

    // ĳ���� ���� ���� ��ų ����

    public void ApplyInitializeSkill()
    {
        if (CurrentRound > Round) return;

        var currentRoundCharacters = battleCharacter[CurrentRound - 1];

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            var characterInfo = currentRoundCharacters[i].GetComponent<CharacterInfo>();
            characterInfo.InitializeSkill(this);
        }
    }

    // ĳ���� isRun and AttackEnd Run �� ��ų ����
    public void ApplyCharacterSkills(int currentRound)
    {
        if (currentRound > Round) return;

        var currentRoundCharacters = battleCharacter[currentRound - 1];

        for (int i = 0; i < currentRoundCharacters.Count; i++)
        {
            var characterInfo = currentRoundCharacters[i].GetComponent<CharacterInfo>();
            characterInfo.ApplySkill(this);
        }
    }

    public void Stage3BossSkill()
    {
        if (MonsterInfo.Tier == "boss")
        {
            var bossSkill = monster.GetComponent<MonsterBossSkill>();
            bossSkill.FinalRoundHealth();
        }
    }

    public void Stage6BossSkill()
    {
        if (MonsterInfo.Tier == "boss" && MonsterInfo.Id == 601)
        {
            RemainingAttack = true;
        }
    }

    public void Stage9BossSkill()
    {
        if (MonsterInfo.Tier == "boss")
        {
            var bossSkill = monster.GetComponent<MonsterBossSkill>();
            bossSkill.NoDamageCurrentRound();
        }
    }

    public void Stage12BossSkill()
    {
        if (MonsterInfo.Tier == "boss")
        {
            var bossSkill = monster.GetComponent<MonsterBossSkill>();
            bossSkill.Stage12BossSkill();
        }
    }

    public void BossSkills()
    {
        Stage3BossSkill();
        Stage9BossSkill();
        Stage12BossSkill();
    }
}
