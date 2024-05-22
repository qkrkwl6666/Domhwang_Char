
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static readonly string DamageSkin = "DamageSkin";
    public static readonly string DamageSkinPath = "Effect/DamageSkin";

    // 데미지 스킨 
    public GameObject DamageSkinPrefab;
    public readonly int DamageSkinInitialcount = 30;
    public GameObject DamageSkinParent;
    public List<GameObject> DamageSkinsInactive {  get; private set; } = new List<GameObject>();
    public List<GameObject> DamageSkinsActive {  get; private set; } = new List<GameObject>();

    private void Awake()
    {
        DamageSkinPrefab = Resources.Load(DamageSkinPath) as GameObject;
        DamageSkinParent = new GameObject();
        DamageSkinParent.name = DamageSkin;

        for (int i = 0; i < DamageSkinInitialcount; i++)
        {
            var skin = Instantiate(DamageSkinPrefab, DamageSkinParent.transform);
            skin.SetActive(false);
            DamageSkinsInactive.Add(skin);
        }
    }


}
