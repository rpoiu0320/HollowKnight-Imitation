using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    private Animator animator;
    private PlayerMover playerMover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        
    }

    private void OnSkill(InputValue value)
    {
        animator.SetTrigger("Skill");
    }

    private void OnAttack(InputValue value)
    {
        if (playerMover.IsDash())     // 대시 중 공격 안됨
            return;

        animator.SetTrigger("Attack");
    }

    private void AttackUp()
    {

    }

    private void JumpAttackDown()
    {

    }
}
