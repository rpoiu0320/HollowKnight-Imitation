using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OldPlayerAttacker : MonoBehaviour
{
    [SerializeField] Transform commonAttackPoint;
    [SerializeField] Transform attackUpPoint;
    [SerializeField] Transform jumpAttackDownPoint;
    [SerializeField] Vector2 commonAttackRange;
    [SerializeField] Vector2 attackUpRange;
    [SerializeField] Vector2 jumpAttackDownRange;
    [SerializeField] Animator commonAttackAnimator;
    [SerializeField] Animator attackUpAnimator;
    [SerializeField] Animator jumpAttackDownAnimator;
    [SerializeField] LayerMask hitMask;
    [SerializeField] bool debug;

    private SoulUI soulUI;
    private Animator animator;
    private OldPlayerMover playerMover;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float knockBackTime;
    private bool isAttack;
    private bool isJumpAttackDown = false;
    public UnityEvent<Collider2D> OnKnockBack;


    private void Awake()
    {
        soulUI = GameObject.Find("SoulGauge").GetComponent<SoulUI>();
        animator = GetComponent<Animator>();
        playerMover = GetComponent<OldPlayerMover>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.LimitMove())     // 대시 중 공격 안됨
            return;

        isAttack = value.isPressed;

        if (isAttack && attackCooldown == 0)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private void MonsterHitAttack(Collider2D collider)
    {
        IHittable hittable = collider.GetComponent<IHittable>();
        hittable?.TakeHit(GameManager.Data.AttackDamage);
        ParticleSystem hitEffect = GameManager.Resource.Instantiate<ParticleSystem>
            ("Prefab/Effect/AttackHitEffect", collider.gameObject.transform.position, GameObject.Find("PoolManager").transform);
        hitEffect.Play();
        GameManager.Data.IncreaseCurSoul();
        soulUI.RenewalCurSoulUI();

        if (collider.tag == "Monster")  // 보스 넉백 방지
            OnKnockBack?.Invoke(collider);
    }

    Coroutine attackRoutine;
    IEnumerator AttackRoutine()
    {
        if (playerMover.InputDir().y > 0)
            AttackUp();
        else if (playerMover.InputDir().y < 0 && !playerMover.IsGround())
            JumpAttackDown();
        else
            CommonAttack();

        while (attackCooldown < 0.3f)
        {
            attackCooldown += Time.deltaTime;

            yield return null;
        }

        attackCooldown = 0;

        yield break;
    }

    private void CommonAttack()
    {
        animator.SetTrigger("Attack");
        commonAttackAnimator.SetTrigger("Attack");
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(commonAttackPoint.position, commonAttackRange, 0, hitMask);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Monster") && collider.gameObject.layer != LayerMask.NameToLayer("Spikes"))
                continue;

            MonsterHitAttack(collider);
        }
    }

    private void AttackUp()
    {
        animator.SetTrigger("Attack");
        attackUpAnimator.SetTrigger("Attack");
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackUpPoint.position, attackUpRange, 0, hitMask);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Monster") && collider.gameObject.layer != LayerMask.NameToLayer("Spikes"))
                continue;

            MonsterHitAttack(collider);
        }
    }

    private void JumpAttackDown()
    {
        animator.SetTrigger("Attack");
        jumpAttackDownAnimator.SetTrigger("Attack");
        isJumpAttackDown = true;
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(jumpAttackDownPoint.position, jumpAttackDownRange, 0, hitMask);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Monster") && collider.gameObject.layer != LayerMask.NameToLayer("Spikes"))
                continue;

            MonsterHitAttack(collider);
            playerKnockBackRoutine = StartCoroutine(PlayerKnockBackRoutine());
        }
    }

    Coroutine playerKnockBackRoutine;
    IEnumerator PlayerKnockBackRoutine()
    {
        knockBackTime = 0;
        
        while (knockBackTime < 0.2f)
        {
            playerMover.StopJumpRoutine();

            if (isJumpAttackDown)
            {
                rb.velocity = Vector3.zero;
                transform.Translate(new Vector3(0, 40 * Time.deltaTime, 0));
            }
            else
            {
                if (playerMover.LastDirX() > 0)
                    transform.Translate(new Vector3(-20 * Time.deltaTime, 0, 0));
                else if (playerMover.LastDirX() < 0)
                    transform.Translate(new Vector3(20 * Time.deltaTime, 0, 0));
            }

            knockBackTime += Time.deltaTime;

            yield return null;
        }

        isJumpAttackDown = false;

        yield break;
    }

    private void OnDrawGizmos()
    {
        if(!debug) 
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(commonAttackPoint.position, commonAttackRange);
        Gizmos.DrawWireCube(attackUpPoint.position, attackUpRange);
        Gizmos.DrawWireCube(jumpAttackDownPoint.position, jumpAttackDownRange);
    }

    public bool IsAttack()
    {
        return isAttack;
    }
}
