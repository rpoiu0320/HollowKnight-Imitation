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

    private Image[] maxHpImages;
    private Stack<Image> curHpImages = new Stack<Image>();

    private void Start()
    {
        maxHpImages = new Image[GameManager.Data.MaxHp];           // Data, UI Manager끼리 충돌 방지를 위해 Awake 대신 Start
        SetUpHp();
    }

    private void SetUpHp()
    {
        for (int i = 0; i < maxHpImages.Length; i++)
        {
            Image hpImage = Instantiate(hpPrefab, transform);           // UI - Hp의 하위로 생성
            hpImage.rectTransform.anchoredPosition                      // 위치 조정
                = new Vector2(hpImage.rectTransform.anchoredPosition.x + 65 * i, hpImage.rectTransform.anchoredPosition.y);

            if (GameManager.Data.CurHp > i)
            {
                hpImage.sprite = onHpImage;
                curHpImages.Push(hpImage);
            }
            else
                hpImage.sprite = nonHpImage;

            hpImage.gameObject.name = $"HpImage{i}";
            maxHpImages[i] = hpImage;
        }
    }

    public void IncreaseCurHpUI()
    {
        curHpImages.Push(maxHpImages[GameManager.Data.CurHp + 1]);
        curHpImages.Peek().sprite = onHpImage; 
    }
    
    public void DecreaseCurHpUI()
    {
        curHpImages.Peek().sprite = onHpImage;
        curHpImages.Pop();
        maxHpImages[GameManager.Data.CurHp].sprite = nonHpImage;
    }
}
