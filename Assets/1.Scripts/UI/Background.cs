using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public readonly string path = "Background/";

    public Image backgroundImage {  get; private set; }

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        StageTable one = DataTableMgr.Instance.Get<StageTable>("Stage");
        string two = one.Get((GameManager.Instance.CurrentStage + 1).ToString()).Background_Id;

        Sprite sp = Resources.Load<Sprite>(path + two);

        backgroundImage.sprite = sp;
    }
}
