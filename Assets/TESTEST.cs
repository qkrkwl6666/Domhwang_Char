using UnityEngine;
using UnityEngine.UI;

public class TESTEST : MonoBehaviour
{
    public UIManager2 uiManager2;
    public GameObject go;
    public RawImage rt;

    // Start is called before the first frame update
    void Start()
    {
        rt.uvRect = uiManager2.AddSlotRenderers(go);
    }

}
