using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectSlot : MonoBehaviour
{
    private Button button;
    public Forming forming;

    public CharacterSlot characterSlot;

    public int SlotIndex { get; set; } 

    private void Awake()
    {
        button = GetComponent<Button>();

        SceneManager.sceneLoaded += OnAwake;
    }

    void Start()
    {
        button.onClick.AddListener(OnRemoveButton);
    }
    private void OnEnable()
    {

    }

    public void OnAwake(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Battle")
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

    }

    public void OnRemoveButton()
    {
        //if (MultiTouchManager.Instance.Tap == false) return;

        if (characterSlot == null) return;

        GameManager.Instance.formationCharacterList[SlotIndex] = null;

        var characterModel = transform.GetChild(0);
        characterModel.transform.SetParent(characterSlot.transform);
        characterModel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -50f, 0f);

        characterSlot.gameObject.SetActive(true);
        characterSlot = null;

        forming.gameStartButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
