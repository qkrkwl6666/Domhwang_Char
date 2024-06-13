using System.Linq;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    public Canvas HpSliderPrefabs;
    public int Id {  get; private set; }
    public string Name { get; private set; }
    [field: SerializeField] public int Hp {  get; set; }
    public int MaxHp {  get; set; }
    public string Tier { get; private set; }
    public bool isDead { get; private set; } = false;
    public int Feature_Id { get; private set; }
    public int Round { get; private set; }
    public int Heal { get; private set; }
    public int Reduced_dmg { get; private set; }
    public int Atk_Effect_Id { get; set; }
    public int SkillId {  get; private set; }
    public bool isInvincible { get; set; }
    public bool isIncreasedDamage { get; set; }
    public MonsterSkillData SkillData { get; private set; }

    private BattleSystem battleSystem;

    public UnityEngine.UI.Slider hpSlider;

    public int CurrentRound { get; set; }

    public bool MonsterAttackEnd { get; private set; } = false;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip hitAudio;

    private void Awake()
    {
        battleSystem = GameObject.FindWithTag("BattleSystem").GetComponent<BattleSystem>();

        SetMonster(GameManager.Instance.MonsterData);

        gameObject.AddComponent<MonsterSkill>();

        if (hpSlider == null)
        {
            var hpCanvas = Instantiate(HpSliderPrefabs, transform);
            hpSlider = hpCanvas.GetComponentInChildren<UnityEngine.UI.Slider>();
            hpCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 2.5f);
        }   

        hpSlider.minValue = 0;
        hpSlider.maxValue = Hp;
        hpSlider.value = hpSlider.maxValue;

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
        SkillId = monsterData.SkillId;

        if(monsterData.SkillId == 0)
        {
            SkillData = null;
            return;
        }

        SkillData = DataTableMgr.Instance.Get<MonsterSkillTable>("MonsterSkill").Get(SkillId.ToString());
    }

    public void Damage(int damage, CharacterInfo ci)
    {
        MonsterSkill monsterSkill = GetComponent<MonsterSkill>();
        
        switch(monsterSkill.currentDamageSkill)
        {
            case DamageSkillStatus.None:
                BasicDamaged(damage);
                break;
            case DamageSkillStatus.FirstGuard:
                monsterSkill.FirstAttackDefence();
                break;
            case DamageSkillStatus.IsRemainingReduceDamage:
                bool isContains = battleSystem.remainingCharacters.Contains(ci.gameObject);
                if (isContains) monsterSkill.RemainingReduceDamage(damage);
                else BasicDamaged(damage);
                break;
            case DamageSkillStatus.IsInvincible:
                {
                    Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
                    DynamicTextManager.CreateText2D(position, "0", DynamicTextManager.defaultData);
                }
                break;
            case DamageSkillStatus.IsRoundIncreasedDamage:
                {
                    if (battleSystem.battleCharacter[battleSystem.CurrentRound - 1]
                        .Contains(ci.gameObject)) monsterSkill.RoundIncreasedDamageReceived(damage);
                    else BasicDamaged(damage);
                }
                break;
        }

        animator.SetTrigger("TakeHit");

        DeadCheck();

        hpSlider.value = Hp;
    }

    public void AttackEnd()
    {
        MonsterAttackEnd = true;
    }

    public void BasicDamaged(int damage)
    {
        Hp -= damage;
        Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
        DynamicTextManager.CreateText2D(position, damage.ToString(), DynamicTextManager.defaultData);
    }

    public void DeathEnd()
    {
        // Todo : 여기서 게임 우승 메서드 호출
        if(GameManager.Instance.CurrentStage >= 11)
        {
            UIManager.Instance.OpenUI(Page.GAMECLEAR);
            return;
        }

        GameManager.Instance.GameWin();
        GameObject.FindWithTag("BackgroundBGM").GetComponent<AudioSource>().Stop();
    }
    public void DeadCheck()
    {
        if (Hp <= 0)
        {
            Hp = 0;
            isDead = true;
            battleSystem.AllStop();
            GameManager.Instance.CharactersCCEnable(false);
            GameManager.Instance.LevelUpCharacterList = battleSystem.battleCharacter[battleSystem.CurrentRound - 1];
            battleSystem.StopAllCoroutines();
            animator.SetTrigger("Death");
        }
    }
}
