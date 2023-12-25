using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;
    private Canvas inGameCanvas;
    public HpUI hpUI;
    public SoulUI soulUI;

    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();

        eventSystem = GameManager.Resource.Instantiate<EventSystem>("Prefab/UI/EventSystem");
        eventSystem.transform.parent = transform;
        eventSystem.gameObject.name = "EventSystem";

        if (scene.name != "TitleScene")
        {
            inGameCanvas = GameManager.Resource.Instantiate<Canvas>("Prefab/UI/Canvas");
            inGameCanvas.gameObject.name = "InGameCanvas";
            inGameCanvas.sortingOrder = 0;

            hpUI = inGameCanvas.GetComponentInChildren<HpUI>();
            soulUI = inGameCanvas.GetComponentInChildren<SoulUI>();
        }
    }   
}
