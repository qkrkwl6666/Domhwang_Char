using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillIcon : MonoBehaviour
{
    public static readonly string SkillIconPrefabPath = "Effect/SkillIcon";

    public GameObject SkillIconPrefab;
    public Transform Content;
    public Image image;

    private void Awake()
    {
        //
        SkillIconPrefab = Resources.Load(SkillIconPrefabPath) as GameObject;
    }
    public void AddSkillIcon(string skillId)
    {
        var skillIcon = Instantiate(SkillIconPrefab, Content);
        skillIcon.GetComponent<Image>().sprite = Resources.Load($"Effect/{skillId}") as Sprite;
    }
}
