using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpSpeed;
    private bool isLook = false;
    private float lookTime = 0;

    private bool pressJumpKey;

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    #region Move and Look

    private void Move()
    {
        if (player.actionLimite)
            return;

        if (player.inputDir.x > 0)
        {
            player.render.flipX = false;
            transform.Translate(new Vector2(moveSpeed * Time.deltaTime, 0));
        }
        else if (player.inputDir.x < 0)
        {
            player.render.flipX = true;
            transform.Translate(new Vector2(-moveSpeed * Time.deltaTime, 0));
        }
        else if (player.inputDir.y != 0)
        {
            player.animator.SetFloat("LookUpDown", player.inputDir.y);
            lookTime += Time.deltaTime;

            if (lookTime > 1.5f && !isLook)
                lookRoutine = StartCoroutine(LookRoutine(player.inputDir.y));
        }
        else if (player.inputDir.y == 0 || player.actionLimite)
        {
            player.animator.SetFloat("LookUpDown", 0);
            lookTime = 0;
        }
    }

    Coroutine lookRoutine;
    IEnumerator LookRoutine(float dirY)
    {
        player.animator.SetBool("IsLook", isLook = true);
        player.camera.UpDownVertical(dirY);

        while (true)
        {
            if (player.inputDir.y == 0 || player.actionLimite)
            {
                player.animator.SetBool("IsLook", isLook = false);

                break;
            }

            yield return null;
        }

        player.camera.ResetVertical();
    }
    #endregion

    #region Dash
    private void OnDash(InputValue value)
    {
        if (player.actionLimite)
            return;

        dashRoutine = StartCoroutine(DashRoutine());
    }

    Coroutine dashRoutine;
    IEnumerator DashRoutine()
    {
        float dashTime = 0;
        player.actionLimite = true;
        gameObject.layer = 1 >> gameObject.layer;
        player.animator.SetTrigger("Dash");

        while (dashTime < 0.5f)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);                // 대시 중 하강 방지

            if (player.render.flipX == false)                                  // 바라보는 방향으로 대시
                player.rb.velocity = new Vector2(dashSpeed, 0);
            else
                player.rb.velocity = new Vector2(-dashSpeed, 0);

            dashTime += Time.deltaTime;

            yield return null;
        }

        player.rb.velocity = new Vector2(0, 0);
        player.actionLimite = false;
        gameObject.layer = 3 << gameObject.layer;
    }
    #endregion

    #region jump
    private void OnJump(InputValue value)
    {
        pressJumpKey = value.isPressed;

        if (pressJumpKey && !player.actionLimite && player.isGround)
            jumpRoutine = StartCoroutine(JumpRoutine());
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {
        float jumpTime = 0;

        while (pressJumpKey && !player.actionLimite)
        {
            if (jumpTime > 0.4f)
                break;
            else
                player.rb.velocity = new Vector2(player.rb.velocity.x, jumpSpeed);        // 일정한 점프속도 보장
            
            jumpTime += Time.deltaTime;

            yield return null;
        }
    }
    #endregion

    #region GroundCheck
    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4f, player.groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 4f, Color.red);

        if (hit.collider != null)
            player.animator.SetBool("IsGround", player.isGround = true);
        else
            player.animator.SetBool("IsGround", player.isGround = false);
    }
    #endregion
}
