using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterChange : MonoBehaviour
{
    public Transform content;
    public GameObject characterSlotPrefab;
    public CardUIInfo SelectedCharacter;

    public Button ChangeButton;
    public Button CencelButton;

    public CardUIInfo removeCharacterCard;

    private CharacterInfo removeCharacterInfo;

    private void Awake()
    {
        ChangeButton.onClick.AddListener(OnChangeButtonClick);
        CencelButton.onClick.AddListener(OnCencelButtonClick);
    }

    private void OnEnable()
    {
        foreach (var character in GameManager.Instance.PlayerCharacterList)
        {
            var go = Instantiate(characterSlotPrefab, content);
            var slot = go.GetComponent<FormationSlot>();
            slot.SetData(character.GetComponent<CharacterInfo>());
        }
    }

    private void OnDisable()
    {
        ChangeButton.interactable = false;
        removeCharacterInfo = null;

        foreach (Transform transform in content.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    public void RemoveCardInfo(CharacterInfo characterInfo)
    {
        removeCharacterInfo = characterInfo;
        removeCharacterCard.SetData(removeCharacterInfo.ConvertCharacterData());
    }

    public void OnChangeButtonClick()
    {
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.OkClip);
        // ���� �÷��̾� ����Ʈ ���� ����

        Destroy(removeCharacterInfo.gameObject);

        GameManager.Instance.PlayerCharacterList.Remove(removeCharacterInfo.gameObject);

        // ī�� ĳ���� ���� �� �÷��̾� ����Ʈ�� �ֱ�
        GameManager.Instance.CreateCharacter(SelectedCharacter.CharacterData);
        GameManager.Instance.TryCount = 3;
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.STAGE);
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));

    }

    public void OnCencelButtonClick()
    {
        GameManager.Instance.BackgroundAudioSource.Stop();
        GameManager.Instance.AudioSource.PlayOneShot(GameManager.Instance.CencelClip);
        GameManager.Instance.TryCount = 3;
        SceneManager.LoadScene("Main");
        UIManager.Instance.OpenUI(Page.STAGE);
        GameManager.Instance.BackgroundAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sound/StageSelect"));
    }
}
