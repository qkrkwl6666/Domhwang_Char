using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunTextEffect : MonoBehaviour
{
    public GameObject RunTextPrefab;

    private List<string> RunTexts = new List<string>();

    private void Awake()
    {
       var table = DataTableMgr.Instance.Get<RunStringTable>("RunString");
            
        foreach(var item in table.runStringTable)
        {
            RunTexts.Add(item.Value.RunString);
        }
    }

    public void CreateRunText(GameObject gameObject)
    {
        int random = Random.Range(0, 101);

        if (random <= 50) return;

        var text = Instantiate(RunTextPrefab, gameObject.transform);
        text.GetComponentInChildren<TextMeshPro>().text = RunTexts[RandomRunTextIndex()];
        text.transform.position = gameObject.transform.position + new Vector3(0f , 1.3f , 0f);
        gameObject.GetComponent<CharacterControll>().textBox = text;
        Destroy(text, 3f);
    }

    public GameObject CreateSkillText(GameObject gameObject, string skillName)
    {
        if (gameObject.GetComponent<CharacterControll>().isRun) return null;

        var text = Instantiate(RunTextPrefab, gameObject.transform);
        text.GetComponentInChildren<TextMeshPro>().text = $"\"{skillName}\"";
        text.GetComponentInChildren<TextMeshPro>().color = Color.red;
        text.transform.position = gameObject.transform.position + new Vector3(0f, 1.5f, 0f);
        gameObject.GetComponent<CharacterControll>().textBox = text;
        Destroy(text, 3f);

        return text;
    }

    public int RandomRunTextIndex()
    {
        return Random.Range(0, RunTexts.Count);
    }
}
