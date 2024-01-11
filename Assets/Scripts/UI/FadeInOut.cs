using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        FadeIn();
    }

    public void FadeIn()
    {
        fadeInRoutine = StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        fadeInRoutine = StartCoroutine(FadeOutRoutine());
    }

    Coroutine fadeInRoutine;
    public IEnumerator FadeInRoutine()
    {
        float a = 1f;

        while (image.color.a > 0)
        {
            image.color = new Color(0, 0, 0, a);
            a -= 0.01f;

            yield return new WaitForEndOfFrame();
        }

        image.enabled = false;
    }


    Coroutine fadeOutRoutine;
    public IEnumerator FadeOutRoutine()
    {
        image.enabled = true;
        float a = 0f; 
        
        while (image.color.a < 1f)
        {
            image.color = new Color(0, 0, 0, a);
            a += 0.01f;

            yield return new WaitForEndOfFrame();
        }
    }
}
