using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;
    public static UIManager instance 
    { 
        get 
        { 
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance; 
        } 
    }

    public Page page;

    public Transform mainPanels;
    [SerializeField] private List<GameObject> defaultPanels = new ();

    private void Awake()
    {
        page = Page.TITLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (Transform panel in mainPanels)
        {
            defaultPanels.Add(panel.gameObject);
            panel.gameObject.SetActive(false);
        }

        defaultPanels[(int)page].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenUI(Page page)
    {
        defaultPanels[(int)this.page].SetActive(false);

        defaultPanels[(int)page].SetActive(true);
        this.page = page;
    }
}
