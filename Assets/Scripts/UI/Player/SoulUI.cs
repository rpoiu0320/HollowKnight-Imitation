using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulUI : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ChangeCurSoul();
    }

    public void ChangeCurSoul()
    {
        rectTransform.anchoredPosition = new Vector2(0, GameManager.Data.CurSoul * 12 - 120);
    }
}
