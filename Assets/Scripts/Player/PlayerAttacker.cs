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
            animator.SetTrigger("Attack");
        }
    }

    private void CommonAttack()
    {

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
