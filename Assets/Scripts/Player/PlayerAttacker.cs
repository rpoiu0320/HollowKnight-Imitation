using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    private Animator animator;
    private PlayerMover playerMover;
    private float attackCooldown;
    private bool isAttack;
    private bool isSkill;
    private bool possibleAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        Debug.Log(isAttack);
        //AttackCooldown();
    }

    private void OnSkill(InputValue value)
    {
        if (playerMover.IsDash())     // 대시 중 공격 안됨
            return;

        isSkill = value.isPressed;

        if (isSkill)       // 이때 공격 콜라이더 생성하면 될거같음
            animator.SetTrigger("Skill");
    }

    private void AttackCooldown()
    {
        if(isAttack)
        {
            return;
        }

        attackCooldown += Time.deltaTime;
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.IsDash())     // 대시 중 공격 안됨
            return;

        isAttack = value.isPressed;

        if (isAttack)       // 이때 공격 콜라이더 생성하면 될거같음
            animator.SetTrigger("Attack");
    }

    private void AttackUp()
    {

    }

    private void JumpAttackDown()
    {

    }
    
    public bool IsAttack()
    {
        return isAttack;
    }

    public bool IsSkill()
    {
        return isSkill;
    }
}
