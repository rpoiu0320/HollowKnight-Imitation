using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulUI : PlayerUI
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private new void Start()
    {
        base.Start();
        ChangeCurSoul();
    }

    public void ChangeCurSoul()
    {
        rectTransform.anchoredPosition = new Vector2(0, player.CurSoul * 12 - 120);
    }
}
