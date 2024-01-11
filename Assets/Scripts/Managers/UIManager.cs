using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;
    private Canvas inGameCanvas;
    public FadeInOut fadeInOut;
    public HpUI hpUI;
    public SoulUI soulUI;

    private void Awake()
    {
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("Prefab/UI/EventSystem");
        eventSystem.transform.parent = transform;
        eventSystem.gameObject.name = "EventSystem";
        inGameCanvas = FindObjectOfType<Canvas>();
        fadeInOut = inGameCanvas.GetComponentInChildren<FadeInOut>();
    }

    public void LoadHpNSoul()
    {
        inGameCanvas = FindObjectOfType<Canvas>();
        fadeInOut = inGameCanvas.GetComponentInChildren<FadeInOut>();
        hpUI = inGameCanvas.GetComponentInChildren<HpUI>();
        soulUI = inGameCanvas.GetComponentInChildren<SoulUI>();
    }
}
