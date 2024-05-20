using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBossSkill : MonoBehaviour
{
    private MonsterInfo monsterInfo;

    private void Awake()
    {
        monsterInfo = GetComponent<MonsterInfo>();
    }

    // 3스테이지 보스 마지막 라운드에 체력을 n 회복
    public void FinalRoundHealth()
    {
        if (monsterInfo.Id == 301 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.Hp += monsterInfo.Heal;
        }
    }

    // 6스테이지 보스 잔류 병사의 데미지가 n 감소
    public void ReducedDamage(int damage)
    {
        if (monsterInfo.Id == 601)
        {
            monsterInfo.isInvincible = true;
        }
    }

    // 9스테이지 보스 3라운드에 대미지를 받지 않음
    public void NoDamageCurrentRound()
    {
        if(monsterInfo.Id == 901 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.isInvincible = true;
        }
        else monsterInfo.isInvincible = false;
    }

    // 12스테이지 보스 3라운드에 돌격하는 병사의 대미지를 8배로 받음
    public void Stage12BossSkill()
    {
        if (monsterInfo.Id == 1201 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.isIncreasedDamage = true;
        }
    }

}
