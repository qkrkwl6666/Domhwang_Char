using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterEliteSkill : MonoBehaviour
{
    public bool Defence { get; private set; } = true;

    public bool FirstAttackDefence()
    {
        if (Defence)
        {
            Vector2 position = transform.position + new Vector3(-1f, 2.5f, 0f);
            DynamicTextManager.CreateText2D(position, "0", DynamicTextManager.defaultData);
            Defence = false;
            return true;
        }

        return false;
    }
}
