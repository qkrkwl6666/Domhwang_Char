using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectSlot : MonoBehaviour
{
    private Image image;
    private Button button;
    public Forming forming;

    public CharacterSlot characterSlot;

    public int SlotIndex { get; set; } 

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(OnRemoveButton);
    }

    public void OnRemoveButton()
    {
        if (MultiTouchManager.Instance.Tap == false) return;

        //forming.uiSelectCharacterList[SlotIndex].get
        //

        if (characterSlot == null) return;

        image.sprite = null;
        GameManager.Instance.formationCharacterList[SlotIndex] = null;
        characterSlot.gameObject.SetActive(true);
        characterSlot = null;

        forming.gameStartButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
