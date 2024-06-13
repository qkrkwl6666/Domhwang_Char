using System;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    private Animator animator;
    private CharacterControll characterControll;
    private CharacterEffect characterEffect;

    public CharacterInfo characterInfo;

    public MonsterInfo monsterInfo;
    public static Action<int, CharacterInfo> MonsterDamageEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterControll = GetComponentInParent<CharacterControll>();
        characterEffect = GetComponentInParent<CharacterEffect>();
    }

    private void AttackDamage()
    {
        //Debug.Log("AttackDamage");
        // 여기서 몬스터 데미지
        //AttackParticle
        string path;
        switch (characterInfo.Tier)
        {
            case "normal":
                path = "Sound/Hit_Normal";
                break;
            case "rare":
                path = "Sound/Hit_Rare";
                break;
            case "epic":
                path = "Sound/Hit_Epic";
                break;
            default:
                path = "Sound/Hit_Normal";
                break;
        }

        GameManager.Instance.AudioSource.PlayOneShot(Resources.Load<AudioClip>(path));
        characterEffect.AttackParticle.Play();
        MonsterDamageEvent?.Invoke(characterInfo.BattleAttack, characterInfo);
    }

    private void AttackEnd()
    {
        animator.SetBool("Attack", false);

        if (monsterInfo.isDead) return;

        if (characterControll.attackEndRun)
        {
            characterControll.AttackEndRunModeChange();
        }
        else
        {
            animator.SetBool("Move", true);

            characterControll.ChangeStatus(CharacterControll.Status.Back);
            characterControll.Flip(false);
        }
        
    }

    public void UpdateMonsterInfo()
    {
        monsterInfo = characterControll.MonsterTransform.gameObject.GetComponent<MonsterInfo>();
    }
}
