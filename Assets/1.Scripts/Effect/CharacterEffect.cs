using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    public static readonly string Effect = "Effect/";
    public ParticleSystem AttackParticle {  get; private set; }
    public ParticleSystem RunParticle { get; private set; }
    public ParticleSystem LeftCryParticle { get; private set; }
    public ParticleSystem RightCryParticle { get; private set; }

    private void Awake()
    {
        //attackParticle = 
    }
    public void EffectInitialiAwake()
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
        LeftCryParticle = go.GetComponent<ParticleSystem>();
        LeftCryParticle.transform.position = new Vector3(0.37f, 0.3f, 0f);

        go = Instantiate(Resources.Load<GameObject>(Effect + cryPath), transform);
        RightCryParticle = go.GetComponent<ParticleSystem>();

        RightCryParticle.transform.localScale = new Vector3(-0.05f, 0.05f, 0.05f);
        RightCryParticle.transform.position = new Vector3(-0.3f, 0.3f, 0f);
    }

    public void PlayCryParticle()
    {
        LeftCryParticle.Play();
        RightCryParticle.Play();
        RunParticle.Play();
    }


    public void EffectAwake()
    {
        AttackParticle.Stop();
        RunParticle.Stop();
        LeftCryParticle.Stop();
        RightCryParticle.Stop();
    }

}
