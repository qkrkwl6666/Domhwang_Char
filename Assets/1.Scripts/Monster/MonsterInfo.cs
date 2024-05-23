using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    public Canvas HpSliderPrefabs;
    public int Id {  get; private set; }
    public string Name { get; private set; }
    public int Hp {  get; set; }
    public int MaxHp {  get; set; }
    public string Tier { get; private set; }
    public bool isDead { get; private set; } = false;
    public int Feature_Id { get; private set; }
    public int Round { get; private set; }
    public int Heal { get; private set; }
    public int Reduced_dmg { get; private set; }
    public int Atk_Effect_Id { get; set; }
    public bool isInvincible { get; set; }
    public bool isIncreasedDamage { get; set; }


    private BattleSystem battleSystem;

    private UnityEngine.UI.Slider hpSlider;

    public int CurrentRound { get; set; }

    public bool MonsterAttackEnd { get; private set; } = false;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip hitAudio;

    private void Awake()
    {
        battleSystem = GameObject.FindWithTag("BattleSystem").GetComponent<BattleSystem>();

        SetMonster(GameManager.Instance.MonsterData);

        if (hpSlider == null)
        {
            var hpCanvas = Instantiate(HpSliderPrefabs, transform);
            hpSlider = hpCanvas.GetComponentInChildren<UnityEngine.UI.Slider>();
            hpCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 2.5f);
        }   

        hpSlider.minValue = 0;
        hpSlider.maxValue = Hp;
        hpSlider.value = hpSlider.maxValue;
        Debug.Log(hpSlider.transform.position);

        transform.position = new Vector3(4f, 0.5f, 0f);

        animator = GetComponent<Animator>();

        CharacterAnimationEvent.MonsterDamageEvent += Damage;
    }

    public void SetMonster(MonsterData monsterData)
    {
        Id = monsterData.Id;
        Hp = monsterData.Hp;
        MaxHp = Hp;
        Feature_Id = monsterData.Feature_Id;
        Heal = monsterData.heal;
        Reduced_dmg = monsterData.reduced_dmg;
        Name = monsterData.Name;
        Tier = monsterData.Tier;
        Round = monsterData.round;
        Atk_Effect_Id = monsterData.Atk_Effect_Id;
    }

    public void Damage(int damage)
    {
        // 엘리트 몬스터 의 경우 첫공격 무효
        if(Tier == "elite")
        {
            if(GetComponent<MonsterEliteSkill>().FirstAttackDefence()) return;
        }

        if(!isInvincible && !isIncreasedDamage && !battleSystem.RemainingAttack)
        {
            Hp -= damage;
            //DynamicTextManager manager = battleSystem.textManager.GetComponent<DynamicTextManager>();
            //manager.c

            Vector2 position = transform.position + new Vector3(-1f , 2.5f, 0f);

            DynamicTextManager.CreateText2D(position, damage.ToString(), DynamicTextManager.defaultData);
        }

        else if (isIncreasedDamage)
        {
            Hp -= damage * 8;
            Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
            DynamicTextManager.CreateText2D(position, (damage * 8).ToString(), DynamicTextManager.defaultData);
        }

        else if (battleSystem.RemainingAttack)
        {
            int reducedDmg = damage - Reduced_dmg;
            if (reducedDmg < 0) { reducedDmg = 0; }

            Hp -= reducedDmg;
        }

        animator.SetTrigger("TakeHit");

        if (Hp <= 0)
        {
            Hp = 0;
            isDead = true;
            GameManager.Instance.CharactersCCEnable(false);
            GameManager.Instance.LevelUpCharacterList = battleSystem.battleCharacter[battleSystem.CurrentRound - 1];
            battleSystem.StopAllCoroutines();
            animator.SetTrigger("Death");
        }

        hpSlider.value = Hp;
    }

    public void AttackEnd()
    {
        MonsterAttackEnd = true;
    }

    public void DeathEnd()
    {
        // Todo : 여기서 게임 우승 메서드 호출
        GameManager.Instance.GameWin();
        GameObject.FindWithTag("BackgroundBGM").GetComponent<AudioSource>().Stop();
    }
}
