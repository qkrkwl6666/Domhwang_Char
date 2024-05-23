using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DataTableMgr dataTableMgr = DataTableMgr.Instance;
        UIManager uIManager = UIManager.Instance;
        GameManager gameManager = GameManager.Instance;
    }

    
}
