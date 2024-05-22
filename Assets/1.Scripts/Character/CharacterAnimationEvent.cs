using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    private Animator animator;
    private CharacterControll characterControll;
    private CharacterEffect characterEffect;

    public CharacterInfo characterInfo;

    public MonsterInfo monsterInfo;
    public static Action<int> MonsterDamageEvent;

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

        characterEffect.AttackParticle.Play();
        MonsterDamageEvent?.Invoke(characterInfo.BattleAttack);
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
