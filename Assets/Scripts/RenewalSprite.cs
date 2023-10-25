using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RenewalSprite : MonoBehaviour
{
    [SerializeField] private SpriteAtlas baseAtlas;         // �������� SpriteAtlas
    [SerializeField] private float delayTime;               // ���� �̹����� ���� �� �� ������ �ð�
    private Sprite[] atlasArray;                            // SpriteAtlas�� �߰��Ǿ��ִ� �̹������� �迭�� ������ ��
    private Image image;                                    // �̹��� ������ �̷������
                                                            // SpriteAtlas�� ���ϴ� �̹����� ����Ϸ��� �ش� �̹����� �̸��� �����ؾ��ϱ⿡ Image�� �迭�� ����
    private void Awake()
    {
        atlasArray = new Sprite[baseAtlas.spriteCount];     // SpriteAtlas�� ũ�⸸ŭ �迭�� ũ�� �Ҵ�
        baseAtlas.GetSprites(atlasArray);                   // SpriteAtlas�� �̹������� �迭�� ����
        image = GetComponent<Image>();
        spriteRoutine = StartCoroutine(SpritesRoutine());
    }

    private void Update()
    {
    }

    Coroutine spriteRoutine;
    /// <summary>
    /// ������ SpriteAtlas�� ����� Sprite���� ������ �����ð��� ���߾� �����ϴ� Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator SpritesRoutine()
    {
        float i = 0;
        int index = 0;

        while (true)
        {
            i += Time.deltaTime;

            if(i > delayTime)
            {
                if (index >= atlasArray.Length)
                    index = 0;

                image.sprite = atlasArray[index++];
                i = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
