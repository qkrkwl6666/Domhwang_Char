using UnityEngine;

public class MonsterEffect : MonoBehaviour
{
    public static readonly string Effect = "Effect/";
    public ParticleSystem AttackParticle { get; private set; }

    public void Awake()
    {
        int atkEffectId = GetComponent<MonsterInfo>().Atk_Effect_Id;
        string atkPath = DataTableMgr.Instance.Get<EffectTable>("Effect").Get(atkEffectId.ToString()).File;
        var go = Instantiate(Resources.Load<GameObject>(Effect + atkPath), new Vector3(-0.5f, 3, 0), Quaternion.identity);
        AttackParticle = go.GetComponent<ParticleSystem>();
    }
}
