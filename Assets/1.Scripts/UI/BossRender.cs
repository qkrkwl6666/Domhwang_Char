using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRender : MonoBehaviour
{
    int stage;

    public Dictionary<string, GameObject> mossModels = new Dictionary<string, GameObject>();

    public string currentMonsterId;

    private void Awake()
    {

        stage = GameManager.Instance.CurrentStage;

        GameObject[] bossModel = Resources.LoadAll<GameObject>(Defines.ResourcesBossModel);

        foreach(var model in bossModel)
        {
            var go = Instantiate(model, transform);
            go.name = model.name;
            go.SetActive(false);

            mossModels.Add(go.name, go);
        }

        //Main.OnMonsterData += OnMonsterModelChange;
    }

    private void OnDestroy()
    {
        //Main.OnMonsterData -= OnMonsterModelChange; 
    }

    private void OnEnable()
    {
        // �� �������� �� ���� ���� ��� ���� ǥ�� 
        // �ٸ� �������� ǥ���� ���� �Ϲ� ���� ǥ��
        currentMonsterId = GameManager.Instance.MonsterData.Id.ToString();
        mossModels[currentMonsterId].SetActive(true);
    }

    private void Start()
    {
        
    }

    public void OnMonsterModelChange(MonsterData data)
    {
        mossModels[currentMonsterId].SetActive(false);
        currentMonsterId = data.Id.ToString();
        mossModels[currentMonsterId].SetActive(true);
    }
}
