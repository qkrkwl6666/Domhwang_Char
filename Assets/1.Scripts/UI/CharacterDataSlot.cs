using UnityEngine;
using UnityEngine.UI;

public class CharacterDataSlot : MonoBehaviour
{
    public Button characterButton;
    
    private CharacterData characterData;
    private CharacterBook characterSelect;
    public Transform modelContent;

    private void Awake()
    {
        
        characterButton = GetComponent<Button>();
        characterButton.onClick.AddListener(OnCharacterButtonClick);
        characterSelect = GetComponentInParent<CharacterBook>();
    }

    public void SetCreateCharacter(CharacterData cd)
    {
        GameObject Model = Resources.Load<GameObject>($"CharacterModel/{cd.Id}");
        var go = Instantiate(Model, modelContent);
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);
        characterData = cd;
    }

    public void OnCharacterButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        characterSelect.UpdateCharacterDescUI(characterData);
    }


}
