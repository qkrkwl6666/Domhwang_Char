using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager uIManager = UIManager.Instance;
        GameManager gameManager = GameManager.Instance;
        DataTableMgr dataTableMgr = DataTableMgr.Instance;
    }
}
