using System.Collections;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject DamageEffectPrefab; 

    private ObjectPool<GameObject> damegePool;

    //public List<GameObject> DamageEffectList {  get; private set; } = new List<GameObject>();


}
