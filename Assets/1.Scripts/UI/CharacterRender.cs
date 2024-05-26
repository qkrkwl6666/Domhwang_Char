using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRender : MonoBehaviour
{
    public List<RenderTexture> characterRenderTexture = new List<RenderTexture>();

    public List<GameObject> characterModels = new List<GameObject>();

    public Camera renderCamera; // �������� ���� ī�޶�

    private void Awake()
    {

        renderCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (GameManager.Instance.PlayerCharacterList.Count != characterRenderTexture.Count)
        {
            Debug.LogError("Character list and render texture list counts do not match!");
            return;
        }

        // �� ĳ���Ϳ� ���� ������ ����
        for (int i = 0; i < characterModels.Count; i++)
        {
            SetupCharacterRender(i);
        }
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        foreach(var character in GameManager.Instance.PlayerCharacterList)
        {
            var cc = character.GetComponent<CharacterInfo>();
            var model = Resources.Load<GameObject>("CharacterModel/" + cc.Id);
            var go = Instantiate(model, transform);
            go.SetActive(false);
        }
    }

    void SetupCharacterRender(int index)
    {
        GameObject character = characterModels[index];
        character.SetActive(true); // ĳ���� Ȱ��ȭ

        character.GetComponent<RectTransform>().localPosition = new Vector3(-0.03f, -0.338f, 0.7f);
        character.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        renderCamera.targetTexture = characterRenderTexture[index]; // �ش� ���� �ؽ�ó�� ī�޶� �Ҵ�

        StartCoroutine(RenderCharacter(character));
    }

    IEnumerator RenderCharacter(GameObject character)
    {
        yield return new WaitForEndOfFrame(); // �� ������ ��ٷ� �������� �Ϸ�ǵ��� ��
        character.SetActive(false); // ������ �� ĳ���� ��Ȱ��ȭ
    }


}
