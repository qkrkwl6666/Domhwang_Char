using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEliteSkill : MonoBehaviour
{
    public bool Defence { get; private set; } = true;

    public bool FirstAttackDefence()
    {
        if (Defence)
        {
            Defence = false;
            return true;
        }

        return false;
    }
}
