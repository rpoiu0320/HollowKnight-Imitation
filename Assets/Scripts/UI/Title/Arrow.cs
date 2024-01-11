using UnityEngine;

public class Arrow : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void MoveStart()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -125);
    }

    public void MoveExit()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -205);
    }
}
