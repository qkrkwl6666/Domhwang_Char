using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        SceneManager.sceneLoaded += FindCanvasChangeScene;
    }

    private void OnEnable()
    {
        canvas.worldCamera = Camera.main;
    }

    public void FindCanvasChangeScene(Scene scene, LoadSceneMode mode)
    {
        if (canvas == null) return;
        canvas.worldCamera = Camera.main;

        if(scene.name == "Main")
        {
            canvas.sortingOrder = 1;
        }
        else if (scene.name == "Battle")
        {
            canvas.sortingOrder = 4;
        }
    }

}
