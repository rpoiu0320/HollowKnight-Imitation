using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] Transform commonAttackPoint;
    [SerializeField] Transform attackUpPoint;
    [SerializeField] Transform jumpAttackDownPoint;
    [SerializeField] Vector2 commonAttackRange;
    [SerializeField] Vector2 attackUpRange;
    [SerializeField] Vector2 jumpAttackDownRange;
    [SerializeField] LayerMask hitMask;

    private Animator animator;
    private PlayerMover playerMover;
    private bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void AttackCooldown()
    {
        if(isAttack)
        {
            return;
        }
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.LimitMove())     // 대시 중 공격 안됨
            return;

        isAttack = value.isPressed;

        if (isAttack)
        {
            if (playerMover.InputDir().y > 0)
                AttackUp();
            else if (playerMover.InputDir().y < 0 && !playerMover.IsGround())
                JumpAttackDown();
            else
                CommonAttack();
        }
    }

    private void CommonAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(commonAttackPoint.position, commonAttackRange, 0, hitMask);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Monster")
            {
                Debug.Log("몬스터가 맞음");
            }
        }
        animator.SetTrigger("Attack");
    }

    private void AttackUp()
    {

    }

    private void JumpAttackDown()
    {

    }

    private void OnDrawGizmos()
    {
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
