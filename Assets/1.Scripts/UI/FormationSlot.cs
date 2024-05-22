using UnityEngine;
using UnityEngine.UI;

public class FormationSlot : MonoBehaviour
{
    //public CardUIInfo card;
    public CharacterInfo characterInfo;
    public CharacterChange CharacterChange { get; private set; }

    private Button Button;

    //public static event Action<CharacterData> CardEvent;

    private void Awake()
    {
        Button = GetComponent<Button>();
        //card = GameObject.FindWithTag("SelectCard").GetComponent<CardUIInfo>();
        Button.onClick.AddListener(OnCharacterClick);
        CharacterChange = GetComponentInParent<CharacterChange>();
    }

    public void SetData(CharacterInfo characterInfo)
    {
        this.characterInfo = characterInfo;
        var go = Resources.Load("CharacterModel/" + characterInfo.Id.ToString()) as GameObject;

        var model = Instantiate(go, transform);
        model.transform.localScale = new Vector3(100f, 100f , 1f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -35f, 0f);
    }

    public void OnCharacterClick()
    {
        // card 에 내 현재 정보 넘기기
        //CardEvent?.Invoke(characterInfo.ConvertCharacterData());
        //card.SetData(characterInfo.ConvertCharacterData());

        CharacterChange.ChangeButton.interactable = true;
        CharacterChange.RemoveCardInfo(characterInfo);
    }

}
