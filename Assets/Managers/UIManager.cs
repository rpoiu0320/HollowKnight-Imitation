using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;

    private Canvas inGameCanvas;

    private void Awake()
    {
        //eventSystem = GameManager.Resource.Instantiate<EventSystem>("UI/EventSystem");
        //eventSystem.transform.parent = transform;
        //eventSystem.gameObject.name = "EventSystem";

        //inGameCanvas = GameManager.Resource.Instantiate<Canvas>("UI/Canvas");
        //inGameCanvas.gameObject.name = "InGameCanvas";
        //inGameCanvas.sortingOrder = 0;
    }
}
