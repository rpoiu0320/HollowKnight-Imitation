using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitter : MonoBehaviour, IHittable
{
    [SerializeField] Animator hitAnimator;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TakeHit(int damage)
    {
        hitRoutine = StartCoroutine(HitRoutine());
    }

    Coroutine hitRoutine;
    IEnumerator HitRoutine()
    {
        hitAnimator.SetTrigger("OnHit");
        player.animator.SetTrigger("Hit");
        GameManager.Data.DecreaseCurHp();
        player.actionLimite = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        twinkleRoutine = StartCoroutine(TwinkleRoutine());
    }

    Coroutine twinkleRoutine;
    IEnumerator TwinkleRoutine()
    {
        int twinklingCount = 3;
        float twinklingTime = 0;

        while (twinklingTime < 1)
        {
            switch (twinklingCount % 3)
            {
                case 0:
                    player.render.color = new Color(0, 0, 0);
                    break;
                case 1:
                    player.render.color = new Color(125, 125, 125);
                    break;
                case 2:
                    player.render.color = new Color(255, 255, 255);
                    break;
                default:
                    break;
            }

            twinklingCount++;

            yield return new WaitForSeconds(0.1f);
        }

        player.render.color = new Color(255, 255, 255);

        yield break;
    }
}
