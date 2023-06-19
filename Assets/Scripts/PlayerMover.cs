using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 inputDir;
    private float jumpTime;
    private bool isJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();

        if (animator.GetBool("IsGround"))
            jumpTime += Time.deltaTime;
        Debug.Log(jumpTime);
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()
    {
        if (inputDir.x > 0)
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        else if (inputDir.x < 0)
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        animator.SetFloat("Move", Mathf.Abs(inputDir.x));

        if (inputDir.x > 0)
            render.flipX = false;
        else if (inputDir.x < 0)
            render.flipX = true;
    }

    private void Jump()
    {
        rb.velocity += Vector2.up * jumpPower * Time.deltaTime;
        //if (rb.velocity.y < 0)
        //    rb.velocity += Vector2.down * Physics2D.gravity.y * Time.deltaTime;
        //else 
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * minJumpPower * Time.deltaTime;
    }

    IEnumerator JumpRoutine()
    {
        Debug.Log("점프루틴 시작");
        while (isJump && jumpTime < 1)
        {
            rb.AddForce(Vector2.up * jumpPower,ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("점프루틴 끝");
    }

    private void OnJump(InputValue value)
    {
        //if (!animator.GetBool("IsGround"))
        //    return;

        Debug.Log("점프 누름");
        isJump = value.isPressed;
        Jump();
        //StartCoroutine(JumpRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.5f, groundLayer);
        
        if (hit.collider != null)
        {
            animator.SetBool("IsGround", true);
            jumpTime = 0;
        }
        else
        {
            animator.SetBool("IsGround", false);
        }
    }
}
