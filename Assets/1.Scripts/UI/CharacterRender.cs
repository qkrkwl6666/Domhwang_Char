using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRender : MonoBehaviour
{
    public List<RenderTexture> characterRenderTexture = new List<RenderTexture>();

    public List<GameObject> characterModels = new List<GameObject>();

    public Camera renderCamera; // 렌더링에 사용될 카메라

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

        // 각 캐릭터에 대해 렌더링 실행
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
        character.SetActive(true); // 캐릭터 활성화

        character.GetComponent<RectTransform>().localPosition = new Vector3(-0.03f, -0.338f, 0.7f);
        character.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        renderCamera.targetTexture = characterRenderTexture[index]; // 해당 렌더 텍스처를 카메라에 할당

        StartCoroutine(RenderCharacter(character));
    }

    IEnumerator RenderCharacter(GameObject character)
    {
        yield return new WaitForEndOfFrame(); // 한 프레임 기다려 렌더링이 완료되도록 함
        character.SetActive(false); // 렌더링 후 캐릭터 비활성화
    }


}
