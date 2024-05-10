using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    private Animator animator;
    private CharacterControll characterControll;
    
    public static event Action<CharacterControll> CharacterRunEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterControll = GetComponentInParent<CharacterControll>();
        
    }

    private void AttackEnd()
    {
        animator.SetBool("Attack", false);

        CharacterRunEvent?.Invoke(characterControll);

        if (characterControll.isRun)
        {
            characterControll.RunModeChange();
        }
        else
        {
            animator.SetBool("Move", true);

            characterControll.ChangeStatus(CharacterControll.Status.Back);
            characterControll.Flip(false);
        }
        
    }
}
