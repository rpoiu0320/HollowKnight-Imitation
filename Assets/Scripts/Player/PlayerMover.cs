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
    private Vector2 lookDir = new Vector2(1, 0);
    private float lookUpDownTime;
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
        Debug.Log(lookUpDownTime);
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
        if (lookDir.y == 0)
            return;

        lookUpDownTime += Time.deltaTime;

        if (inputDir.y > 0)
        {
            isLook = true;
            animator.SetBool("isLook", true);

            if (lookUpDownTime > 0.3f && inputDir.x == 0)
                animator.Play("LookUp");
        }
        else if (inputDir.y < 0)
        {
            isLook = true;
            animator.SetBool("isLook", true);

            if (lookUpDownTime > 0.3f && inputDir.x == 0)
                animator.Play("LookDown");
        }
        else
        {
            lookUpDownTime = 0;
            isLook = false;
            animator.SetBool("isLook", false);
            animator.Play("Idle");
        }

        Debug.Log(inputDir.y);

        animator.SetFloat("LookUpDown", inputDir.y);
    }
    //TODO : 위, 아래 방향키 누르면 그쪽 쳐다봄, 카메라 이동 and 바라보는 방향에 따라 각각 다른 스킬 사용, 스킬 쓸 때 Move 안되게
    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        
        if(inputDir != new Vector2(0, 0) && !isDash)    // 방향키를 누르지 않고 대시할 때 움직이지 않는거 방지, 대시 중 방향 바뀌는거 방지
            lookDir = value.Get<Vector2>();

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
        dashTime = 0f;  // TODO : 대시 애니메이션과 시간 동기화 필요, 점프를 계속 누르고 있으면서 대시를 하면 대시 종료 후 점프하는 현상 수정 필요
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // 대시 중 중력 영향받아 떨어지는거 방지
        animator.SetTrigger("Dash");

        while (true)
        {
            if (lookDir.x > 0)
                transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
            else if (lookDir.x < 0)
                transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));

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

    private void OnDash(InputValue value)
    {
        if (!isDash)    // 대시 중 다시 대시하는거 방지
            dashRoutine = StartCoroutine(DashRoutine());
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

    public bool IsDash()
    {
        return isDash;
    }

    public Vector2 LookDir()
    {
        return lookDir;
    }
}
