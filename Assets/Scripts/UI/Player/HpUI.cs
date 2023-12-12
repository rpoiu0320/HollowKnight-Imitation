using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] Image curHpImage1;
    [SerializeField] Image curHpImage2;
    [SerializeField] Image curHpImage3;
    [SerializeField] Sprite onHpImage;
    [SerializeField] Sprite nonHpImage;

    private Stack<Image> maxHpStack;
    private List<Image> curHpList;
    private int curHpCount;

    private void Awake()
    {
        maxHpStack = new Stack<Image>();
        maxHpStack.Push(curHpImage1);
        maxHpStack.Push(curHpImage2);
        maxHpStack.Push(curHpImage3);
        curHpList = new List<Image>
        {
            curHpImage1,
            curHpImage2,
            curHpImage3
        };
    }

    private void Start()
    {
        curHpCount = GameManager.Data.CurHp;
        AddMaxHp();
    }

    public void AddMaxHp() // 최대체력 증가
    {
        GameManager.Data.MaxHp++;
        Image newHpImage = GameManager.Resource.Instantiate<Image>("Prefab/UI/HpImage", Vector3.zero, GameObject.Find("Hp").transform);
        newHpImage.rectTransform.anchoredPosition = new Vector2(maxHpStack.Peek().rectTransform.anchoredPosition.x + 65, maxHpStack.Peek().rectTransform.anchoredPosition.y);
        newHpImage.name = maxHpStack.Peek().name.Substring(0, maxHpStack.Peek().name.Length - 1) + (maxHpStack.Count + 1);
        maxHpStack.Push(newHpImage);
        curHpList.Add(newHpImage);
        newHpImage.sprite = nonHpImage;
    }

    public void RenewalHpUI()
    {
        if (GameManager.Data.CurHp > curHpCount)
        {
            curHpList[GameManager.Data.CurHp - 1].sprite = onHpImage;
            curHpCount++;
        }
        else if (GameManager.Data.CurHp < curHpCount)
        {
            curHpList[GameManager.Data.CurHp].sprite = nonHpImage;
            curHpCount--;
        }
    }
}
