using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IHittable
{
    private PlayerMover playerMover;
    private SpriteRenderer render;
    private Animator animator;
    private float hitTime;
    private bool isTwinkling;
    private int twinklingCount;
    private int twinklingColor;
    public PlayerData data;
    public UnityEvent OnNoise;

    private void Awake()
    {
        playerMover = GetComponent<PlayerMover>();
        render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void TakeHit(int damage)
    {
        Debug.Log("Player Hit");
        hitRoutine = StartCoroutine(HitRotine());
        OnNoise?.Invoke();
    }

    Coroutine hitRoutine;
    IEnumerator HitRotine()
    {
        hitTime = 0;
        animator.SetTrigger("Hit");
        playerMover.LimitMove(true);
        gameObject.layer = LayerMask.NameToLayer("Default");
        isTwinkling = true;
        twinklingRoutine = StartCoroutine(TwinklingRoutine());
        
        while (hitTime < 0.2f)
        {
            if (playerMover.LastDirX() > 0)
                transform.Translate(new Vector3(-30 * Time.deltaTime, 0));
            else if (playerMover.LastDirX() < 0)
                transform.Translate(new Vector3(30 * Time.deltaTime, 0));

            hitTime += Time.deltaTime;

            yield return null;
        }

        playerMover.LimitMove(false);

        yield return new WaitForSeconds(0.8f);

        isTwinkling = false;
        twinklingColor = 255;
        render.color = new Color(twinklingColor, twinklingColor, twinklingColor);
        gameObject.layer = LayerMask.NameToLayer("Player");

        yield break;
    }

    Coroutine twinklingRoutine;
    IEnumerator TwinklingRoutine()
    {
        twinklingCount = 3;

        while(isTwinkling)
        {
            switch(twinklingCount % 3)
            {
                case 0:
                    twinklingColor = 0; 
                    break;
                case 1:
                    twinklingColor = 125;
                    break;
                case 2:
                    twinklingColor = 255;
                    break;
                default:
                    break;
            }

            twinklingCount++;
            render.color = new Color(twinklingColor, twinklingColor, twinklingColor);

            yield return new WaitForSeconds(0.1f);
        }

        twinklingColor = 255;
        render.color = new Color(twinklingColor, twinklingColor, twinklingColor);

        yield break;
    }
}
