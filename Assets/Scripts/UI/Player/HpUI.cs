using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] Image hpPrefab;
    [SerializeField] Sprite onHpImage;
    [SerializeField] Sprite nonHpImage;

    private Image[] hpImages; 

    private void Start()
    {
        hpImages = new Image[GameManager.Data.MaxHp];           // Data, UI Manager���� �浹 ������ ���� Awake ��� Start
        SetUpHp();
    }

    private void SetUpHp()
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            Image hpImage = Instantiate(hpPrefab, transform);           // UI - Hp�� ������ ����
            hpImage.rectTransform.anchoredPosition                      // ��ġ ����
                = new Vector2(hpImage.rectTransform.anchoredPosition.x + 65 * i, hpImage.rectTransform.anchoredPosition.y);

            if (GameManager.Data.CurHp > i)
                hpImage.sprite = onHpImage;
            else
                hpImage.sprite = nonHpImage;

            hpImage.gameObject.name = $"HpImage{i}";
            hpImages[i] = hpImage;
        }
    }

    public void IncreaseCurHpUI()
    {
        hpImages[GameManager.Data.CurHp + 1].sprite = onHpImage;
    }
    
    public void DecreaseCurHpUI()
    {
        hpImages[GameManager.Data.CurHp].sprite = nonHpImage;
    }
}
