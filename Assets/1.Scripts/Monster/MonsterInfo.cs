using UnityEngine;


public class MonsterInfo : MonoBehaviour
{
    public Canvas HpSliderPrefabs;

    public int Id {  get; private set; }
    [field:SerializeField] public int Hp {  get; private set; }
    public bool isDead { get; private set; } = false;
    public int Feature_Id { get; private set; }
    public int Heal { get; private set; }
    public int Reduced_dmg { get; private set; }

    private BattleSystem battleSystem;

    private UnityEngine.UI.Slider hpSlider;

    public bool MonsterAttackEnd { get; private set; } = false;
    private Animator animator;

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
        Feature_Id = monsterData.Feature_Id;
        Heal = monsterData.heal;
        Reduced_dmg = monsterData.reduced_dmg;
    }

    public void Damage(int damage)
    {
        Hp -= damage;
        animator.SetTrigger("TakeHit");

        if (Hp <= 0)
        {
            Hp = 0;
            isDead = true;
            GameManager.Instance.CharactersCCEnable(false);
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
    }
}
