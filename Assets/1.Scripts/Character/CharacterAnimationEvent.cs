using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    private Animator animator;
    private CharacterControll characterControll;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterControll = GetComponentInParent<CharacterControll>();
        
    }

    private void AttackEnd()
    {
        Debug.Log("AttackEnd");

        animator.SetBool("Attack", false);

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
}
