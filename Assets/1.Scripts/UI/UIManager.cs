using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public Page page;

    private Canvas canvas;
    private Transform mainPanels;
    [SerializeField] private List<GameObject> defaultPanels = new ();

    private void Awake()
    {
        mainPanels = GameObject.FindWithTag("MainPanel").transform;
        canvas = mainPanels.GetComponentInParent<Canvas>();
        
        //Debug.Log("UIManagerAwake");
        page = Page.TITLE;

        DontDestroyOnLoad(canvas.gameObject);

        foreach (Transform panel in mainPanels)
        {
            defaultPanels.Add(panel.gameObject);
            panel.gameObject.SetActive(false);
        }

        defaultPanels[(int)page].SetActive(true);

        //SceneManager.sceneLoaded += CanvasRemove;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += CanvasRemove;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CanvasRemove;
    }

    // Update is called once per frame
    void Update()   
    {
        
    }

    public void OpenUI(Page page)
    {
        if(page == Page.OPTION)
        {
            defaultPanels[(int)page].SetActive(true);
            return;
        }

        defaultPanels[(int)this.page].SetActive(false);
        defaultPanels[(int)page].SetActive(true);
        this.page = page;
    }
    public void AllClose()
    {
        for(int i = 0; i < defaultPanels.Count; i++)
        {
            defaultPanels[i].SetActive(false);
        }
    }

    public void CanvasRemove(UnityEngine.SceneManagement.Scene scene , LoadSceneMode mode)
    {
        if(scene.name == "Main")
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("MainCanvas");

            foreach(GameObject obj in objects)
            {
                if(obj.GetComponent<Canvas>() != canvas) 
                {
                    Destroy(obj);
                }
            }
        }
    }
}
