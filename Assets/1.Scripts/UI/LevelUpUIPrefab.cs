using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpUIPrefab : MonoBehaviour
{
    public GameObject characterModel;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI runText;
    public List<Sprite> linePanel = new List<Sprite>();

    public void SetLevelUpUI(CharacterInfo ci)
    {
        var go = Resources.Load("CharacterModel/" + ci.Id.ToString()) as GameObject;
        var model = Instantiate(go, characterModel.transform);
        model.transform.localScale = new Vector3(100f, 100f, 100f);
        model.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -40f, 0f);

        levelText.text = $"Level{ci.Level}";
        attackText.text = $"°ø°Ý·Â : {ci.Atk} +{ci.Atk_Up}";
        runText.text = $"µµ¸Á È®·ü : {ci.Run} -{ci.Run_Up}";
    }

}
