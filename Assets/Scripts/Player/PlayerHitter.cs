using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHitter : MonoBehaviour, IHittable
{
    [SerializeField] private Animator hitAnimator;

    private Vector2 LastStep { get { return player.lastStep; } }
    private Animator Animator { get { return player.animator; } }
    private SpriteRenderer Render { get { return player.render; } }
    private bool ActionLimite { set { player.actionLimite = value; } }

    private Player player;
    private LayerMask playerLayer;
    
    public UnityEvent OnCameraNoise;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void TakeHit(int damage)
    {
        hitRoutine = StartCoroutine(HitRoutine());
    }

    public void HitBySpikes(int damage)
    {
        hitRoutine = StartCoroutine(HitRoutine());
        locationRenwalRoutine = StartCoroutine(LocationRenwalRoutine());
    }

    Coroutine locationRenwalRoutine;
    IEnumerator LocationRenwalRoutine()
    {
        GameManager.UI.fadeInOut.FadeOut();

        yield return new WaitForSeconds(1f);

        GameManager.UI.fadeInOut.FadeIn();

        transform.position = LastStep;
    }

    Coroutine hitRoutine;
    IEnumerator HitRoutine()
    {
        gameObject.layer = gameObject.layer >> 2;
        float knockBackTime = 0;
        hitAnimator.SetTrigger("OnHit");
        Animator.SetTrigger("Hit");
        GameManager.Data.DecreaseCurHp();
        ActionLimite = true;
        twinkleRoutine = StartCoroutine(TwinkleRoutine());
        
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(0.2f);

        Time.timeScale = 1;
        OnCameraNoise?.Invoke();

        while (knockBackTime < 0.2f)
        {
            PlayerKnockBack();
            knockBackTime += Time.deltaTime;

            yield return null;
        }

        ActionLimite = false;
    }

    private void PlayerKnockBack()
    {
        if (Render.flipX)
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
                    Render.color = new Color(0, 0, 0);
                    break;
                case 1:
                    Render.color = new Color(125, 125, 125);
                    break;
                case 2:
                    Render.color = new Color(255, 255, 255);
                    break;
                default:
                    break;
            }

            twinklingCount++;
            twinklingTime += Time.deltaTime;

            yield return new WaitForSeconds(0.1f);
        }

        Render.color = new Color(255, 255, 255);
        gameObject.layer = playerLayer;
    }
}
