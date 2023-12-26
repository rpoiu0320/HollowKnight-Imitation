using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHitter : MonoBehaviour, IHittable
{
    [SerializeField] Animator hitAnimator;
    private Player player;
    private LayerMask playerLayer;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void TakeHit(int damage)
    {
        hitRoutine = StartCoroutine(HitRoutine());
    }

    Coroutine hitRoutine;
    IEnumerator HitRoutine()
    {
        float knockBackTime = 0;
        gameObject.layer = gameObject.layer >> 2;
        hitAnimator.SetTrigger("OnHit");
        player.animator.SetTrigger("Hit");
        GameManager.Data.DecreaseCurHp();
        player.actionLimite = true;
        twinkleRoutine = StartCoroutine(TwinkleRoutine());
        
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;

        while (knockBackTime < 0.2f)
        {
            PlayerKnockBack();
            knockBackTime += Time.deltaTime;

            yield return null;
        }

        player.actionLimite = false;
    }

    private void PlayerKnockBack()
    {
        if (player.render.flipX)
            transform.Translate(new Vector3(30 * Time.deltaTime, 0));
        else
            transform.Translate(new Vector3(-30 * Time.deltaTime, 0));
    }

    Coroutine twinkleRoutine;
    IEnumerator TwinkleRoutine()
    {
        int twinklingCount = 3;
        float twinklingTime = 0;

        while (twinklingTime < 0.1f)
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
            twinklingTime += Time.deltaTime;

            yield return new WaitForSeconds(0.1f);
        }

        player.render.color = new Color(255, 255, 255);
        gameObject.layer = playerLayer;
    }
}
