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

    // 3�������� ���� ������ ���忡 ü���� n ȸ��
    public void FinalRoundHealth()
    {
        if (monsterInfo.Id == 301 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.Hp += monsterInfo.Heal;
        }
    }

    // 6�������� ���� �ܷ� ������ �������� n ����
    public void ReducedDamage(int damage)
    {
        if (monsterInfo.Id == 601)
        {
            monsterInfo.isInvincible = true;
        }
    }

    // 9�������� ���� 3���忡 ������� ���� ����
    public void NoDamageCurrentRound()
    {
        if(monsterInfo.Id == 901 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.isInvincible = true;
        }
        else monsterInfo.isInvincible = false;
    }

    // 12�������� ���� 3���忡 �����ϴ� ������ ������� 8��� ����
    public void Stage12BossSkill()
    {
        if (monsterInfo.Id == 1201 && monsterInfo.CurrentRound == monsterInfo.Round)
        {
            monsterInfo.isIncreasedDamage = true;
        }
    }

}
