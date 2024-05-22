using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    public static readonly string Effect = "Effect/";
    public ParticleSystem AttackParticle {  get; private set; }
    public ParticleSystem RunParticle { get; private set; }
    public ParticleSystem CryParticle { get; private set; }

    private void Awake()
    {
        //attackParticle = 
    }
    public void EffectAwake()
    {
        int atkEffectId = GetComponent<CharacterInfo>().Atk_Effect_Id;
        int runEffectId = GetComponent<CharacterInfo>().Run_Effect_Id;
        int cryEffectId = GetComponent<CharacterInfo>().Cry_Effect_Id;

        string atkPath = DataTableMgr.Instance.Get<EffectTable>("Effect").Get(atkEffectId.ToString()).File;
        string runPath = DataTableMgr.Instance.Get<EffectTable>("Effect").Get(runEffectId.ToString()).File;
        string cryPath = DataTableMgr.Instance.Get<EffectTable>("Effect").Get(cryEffectId.ToString()).File;

        var go = Instantiate(Resources.Load<GameObject>(Effect + atkPath), new Vector3(3.2f, 0.9f , 0), Quaternion.identity);
        DontDestroyOnLoad(go);

        AttackParticle = go.GetComponent<ParticleSystem>();

        go = Instantiate(Resources.Load<GameObject>(Effect + runPath), transform);

        RunParticle = go.GetComponent<ParticleSystem>();
        RunParticle.transform.position = new Vector3(-0.4f, 0.2f, 0f); 

        go = Instantiate(Resources.Load<GameObject>(Effect + cryPath), transform);
        CryParticle = go.GetComponent<ParticleSystem>();
        CryParticle.transform.position = new Vector3(0.4f, 0.2f, 0f);
    }

    public void Flip(bool isFlip)
    {

    }

}
