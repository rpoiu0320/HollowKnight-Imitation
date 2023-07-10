using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
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

    private Player player;
    private Animator animator;
    private PlayerMover playerMover;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float knockBackTime;
    private bool isAttack;
    private bool isJumpAttackDown = false;
    public UnityEvent<Collider2D> OnKnockBack;


    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
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

            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit(player.data.Player[0].attackDamage);
            ParticleSystem hitEffect = GameManager.Resource.Instantiate<ParticleSystem>
                ("Prefab/Effect/AttackHitEffect", collider.gameObject.transform.position, GameObject.Find("PoolManager").transform);
            hitEffect.Play();

            if (collider.tag == "Monster")  // 보스 넉백 방지
                OnKnockBack?.Invoke(collider);
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

            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit(player.data.Player[0].attackDamage);
            ParticleSystem hitEffect = GameManager.Resource.Instantiate<ParticleSystem>
                ("Prefab/Effect/AttackHitEffect", collider.gameObject.transform.position, GameObject.Find("PoolManager").transform);
            hitEffect.Play();

            if (collider.tag == "Monster")
                OnKnockBack?.Invoke(collider);
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
            
            IHittable hittable = collider.GetComponent<IHittable>();
            hittable?.TakeHit(player.data.Player[0].attackDamage);
            ParticleSystem AttackhitEffect = GameManager.Resource.Instantiate<ParticleSystem>
                ("Prefab/Effect/AttackHitEffect", collider.gameObject.transform.position, GameObject.Find("PoolManager").transform);
            AttackhitEffect.Play();

            if (collider.tag == "Monster")
                OnKnockBack?.Invoke(collider);

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
