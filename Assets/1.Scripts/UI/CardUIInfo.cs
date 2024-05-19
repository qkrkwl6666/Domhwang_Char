using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardUIInfo : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI run;
    public TextMeshProUGUI skill;
    public Transform character;

    private void Awake()
    {
        //FormationSlot.CardEvent += SetData;
    }

    public void SetData(CharacterData characterData)
    {
        foreach(Transform t in character)
        {
            Destroy(t.gameObject);
        }

        characterName.text = characterData.Name;
        level.text = characterData.Level.ToString();
        attack.text = characterData.Atk.ToString();
        run.text = characterData.Run.ToString();
        skill.text = characterData.Skill_Id.ToString();

        var go = Resources.Load("CharacterModel/" + characterData.Id.ToString()) as GameObject;

        var model = Instantiate(go, character);

        model.transform.localScale = new Vector3(150f, 150f, 1f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);
    }
}
