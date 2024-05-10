using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour
{
    public Canvas HpSliderPrefabs;
    public int Hp {  get; private set; }
    public bool isDead { get; private set; } = false;

    private Slider hpSlider;
    private void Awake()
    {
        Hp = 30;

        if(hpSlider == null)
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
