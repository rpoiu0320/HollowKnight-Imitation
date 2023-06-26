using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 inputDir;
    private Vector2 dashDir = new Vector2(1, 0);
    private float jumpTime;
    private float dashTime;
    private bool isLook;
    private bool isJump;
    private bool isGround;
    private bool isDash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        LookUpDown();

        if (!isDash) 
            Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if (inputDir.x > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
        }
            
        else if (inputDir.x < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
        }
    }

    private void LookUpDown()
    {
        if (inputDir.y > 0)
        {
            isLook = true;
            animator.SetBool("isLook", true);
        }
        else if (inputDir.y < 0)
        {
            isLook = true;
            animator.SetBool("isLook", true);
        }
        else
        {
            isLook = false;
            animator.SetBool("isLook", false);
        }

        animator.SetFloat("LookUpDown", inputDir.y);
    }
    //TODO : 위, 아래 방향키 누르면 그쪽 쳐다봄, 카메라 이동 and 바라보는 방향에 따라 각각 다른 스킬 사용
    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        
        if(inputDir != new Vector2(0, 0) && !isDash)
        {
            dashDir = value.Get<Vector2>();
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()
    {

        while (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;

            if (jumpTime > 1f)
            {
                break;
            }

            yield return null;
        }
    }

    private void OnJump(InputValue value)
    {        
        isJump = value.isPressed;

        if (isJump) 
            jumpTime = 0f;

        if (isJump && isGround)
        {
            jumpRoutine = StartCoroutine(JumpRoutine());
        }

        if (!isJump)
        {
            StopCoroutine(jumpRoutine);
        }
    }

    Coroutine dashRoutine;
    IEnumerator DashRoutine()
    {
        isDash = true;
        dashTime = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        animator.SetTrigger("Dash");

        //if (dashDir.x > 0)
        //        transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
        //    else if (dashDir.x < 0)
        //        transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));   

        while (true)
        {
            if (dashDir.x > 0)
                transform.Translate(new Vector3(dashSpeed * Time.deltaTime, 0, 0));
            else if (dashDir.x < 0)
                transform.Translate(new Vector3(-dashSpeed * Time.deltaTime, 0, 0));

            dashTime += Time.deltaTime;

            if(dashTime > 0.3f)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isDash = false;
                break;
            }
            yield return null;
        }
    }

    public bool IsDash()
    {
        return isDash;
    }

    private void OnDash(InputValue value)
    {
        if (!isDash)
        {
            dashRoutine = StartCoroutine(DashRoutine());
            
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 5f, Color.red);

        if (hit.collider != null)
        {
            isGround = true;
            animator.SetBool("IsGround", true);
        }
        else
        {
            isGround = false;
            animator.SetBool("IsGround", false);
        }
    }
}
