using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    public enum UpDown { None, Up, Down }

    private PlayerAttacker playerAttacker;
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
    private bool isCameraMove;
    private UpDown upDown;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        DefaultLook();
    }

    private void Update()
    {
        if (!isDash) 
            Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()     // 실질적 이동
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

    Coroutine lookingRoutine;
    IEnumerator LookingRoutine()
    {
        while (isLook)
        {
            if (playerAttacker.IsAttack())      // 위, 아래를 쳐다보던 중 공격을 하면 시점을 재자리로 복귀, 
            {                                   // 공격이 진행 및 끝나도 계속 쳐다보고 있으면(위, 아래 키를 계속 누르고 있으면) 그 방향을 다시 바라보게됨
                DefaultLook();

                yield return new WaitUntil(() => playerAttacker.IsAttack() == false);
            }

            if (!isGround)                      // 공중에서 위, 아래로 시점이동 방지
            {
                DefaultLook();

                yield return new WaitUntil(() => isGround == true);
            }

            lookUpDownTime += Time.deltaTime;

            if (inputDir.x != 0 || inputDir.y == 0)     // 위, 아래를 보던 중 이동을 하거나 위, 아래 키를 때면 시점 초기화 
            {
                DefaultLook();
                isLook = false;
                animator.SetFloat("LookUpDown", inputDir.y);
                break;
            }
            else if (inputDir.y > 0)        // TODO : 시점이 이동될 시간, 애니메이션이 움직일 시간 등 추후 수정 필요할거같음
            { 
                animator.SetBool("isLook", true);
                animator.SetFloat("LookUpDown", inputDir.y);

                if (inputDir.x == 0)
                {
                    if(lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Up;
                        isCameraMove = true;
                    }
                }
            }
            else if (inputDir.y < 0)
            {
                animator.SetBool("isLook", true);
                animator.SetFloat("LookUpDown", inputDir.y);

                if (inputDir.x == 0)
                {
                    if (lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Down;
                        isCameraMove = true;
                    }
                }
            }
            yield return null;
        }
    }

    private void DefaultLook()      // Look과 관련된 사항들 초기화
    {
        lookUpDownTime = 0;
        animator.SetBool("isLook", false);
        animator.Play("Idle");
        upDown = UpDown.None;
        isCameraMove = false;
    }

    //TODO : 스킬 쓸 때 Move 안되게
    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        
        if(inputDir != new Vector2(0, 0) && !isDash)    // 방향키를 누르지 않고 대시할 때 움직이지 않는거 방지, 대시 중 방향 바뀌는거 방지
            lookDir = value.Get<Vector2>();

        if(inputDir.y != 0)
        {
            isLook = true;
            lookingRoutine = StartCoroutine(LookingRoutine());
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()       // 점프 끝나고 내려올 때 
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

    // 이하 함수들은 다른 컴포넌트들과 상호작용

    public UpDown LookingUpDown()
    {
        return upDown;
    }

    public Vector2 LookDir()
    {
        return lookDir;
    }

    public Vector2 InputDir()
    {
        return inputDir;
    }

    public bool IsLook()
    {
        return isLook;
    }

    public bool IsDash()
    {
        return isDash;
    }

    public bool IsCameraMove()
    {
        return isCameraMove;
    }
}
