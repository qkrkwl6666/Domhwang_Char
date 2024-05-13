using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static MonsterTable;

public class MonsterInfo : MonoBehaviour
{
    public Canvas HpSliderPrefabs;

    public int Id {  get; private set; }
    public int Hp {  get; private set; }
    public bool isDead { get; private set; } = false;
    public int Feature_Id { get; private set; }
    public int Heal { get; private set; }
    public int Reduced_dmg { get; private set; }


    private Slider hpSlider;
    private void Awake()
    {
        SetMonster(GameManager.Instance.MonsterData);

        if (hpSlider == null)
        {
            var hpCanvas = Instantiate(HpSliderPrefabs, transform);
            hpSlider = hpCanvas.GetComponentInChildren<Slider>();
        }

        hpSlider.minValue = 0;
        hpSlider.maxValue = Hp;
        hpSlider.value = hpSlider.maxValue;
    }

    private void Update()
    {

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
        
        if (Hp <= 0)
        {
            Hp = 0;
            isDead = true;
            Debug.Log("Dead");
        }

        hpSlider.value = Hp;
    }
}
