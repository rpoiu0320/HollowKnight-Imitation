using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : PlayerUI
{
    [SerializeField] Image curHpImage1;
    [SerializeField] Image curHpImage2;
    [SerializeField] Image curHpImage3;
    [SerializeField] Sprite OnHpImage;
    [SerializeField] Sprite NonHpImage;
    private Stack<Image> maxHp;
    private List<Image> curHp;

    private void Awake()
    {
        maxHp = new Stack<Image>();
        maxHp.Push(curHpImage1);
        maxHp.Push(curHpImage2);
        maxHp.Push(curHpImage3);
        curHp = new List<Image>();
        curHp.Add(curHpImage1);
        curHp.Add(curHpImage2);
        curHp.Add(curHpImage3);
    }

    private new void Start()
    {
        base.Start();
        AddMaxHp();
    }

    public void AddMaxHp() // 최대체력 증가
    {
        player.MaxHp++;
        Image newHpImage = GameManager.Resource.Instantiate<Image>("Prefab/UI/HpImage", Vector3.zero, GameObject.Find("Hp").transform);
        newHpImage.rectTransform.anchoredPosition = new Vector2(maxHp.Peek().rectTransform.anchoredPosition.x + 65, maxHp.Peek().rectTransform.anchoredPosition.y);
        newHpImage.name = maxHp.Peek().name.Substring(0, maxHp.Peek().name.Length - 1) + (maxHp.Count + 1);
        maxHp.Push(newHpImage);
        curHp.Add(newHpImage);

        if (player.CurHp < player.MaxHp)
            newHpImage.sprite = NonHpImage;
    }

    public void DecreaseHp()
    {
        curHp[player.CurHp - 1].sprite = NonHpImage;
        player.DecreaseCurHp();
    }
}
