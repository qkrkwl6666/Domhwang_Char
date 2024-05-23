using UnityEngine;

public class MonsterEffect : MonoBehaviour
{
    public static readonly string Effect = "Effect/";
    public ParticleSystem AttackParticle { get; private set; }

    public void Awake()
    {
        int atkEffectId = GetComponent<MonsterInfo>().Atk_Effect_Id;
        string atkPath = DataTableMgr.Instance.Get<EffectTable>("Effect").Get(atkEffectId.ToString()).File;

        // Todo : 나중에 고쳐
        switch (GetComponent<MonsterInfo>().Atk_Effect_Id)
        {
            case 301:
                {
                    var go = Instantiate(Resources.Load<GameObject>(Effect + atkPath), new Vector3(-0.5f, 3, 0), Quaternion.identity);
                    AttackParticle = go.GetComponent<ParticleSystem>();
                }
                break;
            case 302:
                {
                    var go = Instantiate(Resources.Load<GameObject>(Effect + atkPath), new Vector3(-0.5f, 3, 0), Quaternion.identity);
                    AttackParticle = go.GetComponent<ParticleSystem>();
                }
                break;
            case 303:
                {
                    var go = Instantiate(Resources.Load<GameObject>(Effect + atkPath), new Vector3(-0.5f, 1, 0), Quaternion.identity);
                    AttackParticle = go.GetComponent<ParticleSystem>();
                }
                break;
        }


    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    AttackParticle.Play();
        //}
    }
}
