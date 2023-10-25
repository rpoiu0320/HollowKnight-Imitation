using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class RenewalSprite : MonoBehaviour
{
    [SerializeField] private SpriteAtlas baseAtlas;         // 만들어놓은 SpriteAtlas
    [SerializeField] private float delayTime;               // 다음 이미지로 변경 될 때 까지의 시간
    private Sprite[] atlasArray;                            // SpriteAtlas에 추가되어있는 이미지들을 배열에 저장할 곳
    private Image image;                                    // 이미지 변경이 이루어질곳
                                                            // SpriteAtlas는 원하는 이미지를 사용하려면 해당 이미지의 이름을 지정해야하기에 Image를 배열로 관리
    private void Awake()
    {
        atlasArray = new Sprite[baseAtlas.spriteCount];     // SpriteAtlas의 크기만큼 배열의 크기 할당
        baseAtlas.GetSprites(atlasArray);                   // SpriteAtlas의 이미지들을 배열에 저장
        image = GetComponent<Image>();
        spriteRoutine = StartCoroutine(SpritesRoutine());
    }

    private void Update()
    {
    }

    Coroutine spriteRoutine;
    /// <summary>
    /// 지정한 SpriteAtlas에 저장된 Sprite들을 지정한 지연시간에 맞추어 변경하는 Coroutine
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
