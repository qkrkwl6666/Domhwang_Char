using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillIcon : MonoBehaviour
{
    public static readonly string SkillIconPrefabPath = "Effect/SkillIcon";

    public static readonly string ContentbPath = "SkillIconCanvas/Scroll View/Viewport/Content";

    public GameObject SkillIconPrefab;
    public Transform Content;
    public Image image;

    private void Awake()
    {
        SkillIconPrefab = Resources.Load(SkillIconPrefabPath) as GameObject;

        Content = transform.Find(ContentbPath).transform;
    }

    private void Update()
    {

    }

    public void AddSkillIcon(string skillId)
    {
        var skillIcon = Instantiate(SkillIconPrefab, Content);
        skillIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Effect/Skill_Icons/{skillId}");
    }
    private void OnDisable()
    {
        foreach(Transform t in Content)
        {
            Destroy(t.gameObject);
        }
    }
}
