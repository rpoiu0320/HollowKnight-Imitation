using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldPlayerMover : MonoBehaviour
{
    [SerializeField] GameObject dashEffect;
    [SerializeField] Transform commonAttackPoint;
    [SerializeField] SpriteRenderer commonAttackRen;
    [SerializeField] SpriteRenderer attackUpRen;
    [SerializeField] SpriteRenderer jumpAttackDownRen;
    [SerializeField] float moveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundLayer;

    public enum UpDown { None, Up, Down }
    public enum DirX { Left, Right }

    private OldPlayerAttacker playerAttacker;
    private Rigidbody2D rb;
    private Animator animator;
    private Animator dashAnimator;
    private SpriteRenderer render;
    private SpriteRenderer dashRender;
    private Vector2 inputDir;
    private Vector2 lookDir = new Vector2(1, 0);
    private UpDown upDown;
    private DirX dirX;
    private DirX lastDirXCheck;
    private float dashDir;                         // 방향키 입력 없으면 제자리 대시하는 현상 방지
    private float lookUpDownTime;
    private float jumpTime;
    private float dashTime;
    private float originCommonAttackPointX;
    private bool limitMove;
    private bool isDash;
    private bool isLook;
    private bool isJump;
    private bool isGround;
    private bool isCameraMove;

    private void Awake()
    {
        playerAttacker = GetComponent<OldPlayerAttacker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashAnimator = dashEffect.GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        dashRender = dashEffect.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        dashRender.flipX = true;
        dashDir = lookDir.x;
        originCommonAttackPointX = commonAttackPoint.localPosition.x;
        dirX = DirX.Right;
        lastDirXCheck = DirX.Right;
    }

    private void Update()
    {
        //Debug.Log(inputDir + " inputDir");
        //Debug.Log(lookDir + " lookDir");
        //Debug.Log(dashDir + " dashDir");

        if (!limitMove) 
            Move();

        if (dirX != lastDirXCheck)
        {
            dirX = lastDirXCheck;
            ChangeAttackPositionX();
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()     // 실질적 이동
    {
        if (limitMove)
            return;

        if (inputDir.x > 0)
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = false;
            dashRender.flipX = true;
            commonAttackRen.flipX = false;
            attackUpRen.flipX = false;
            jumpAttackDownRen.flipX = false;
            lastDirXCheck = DirX.Right;
        }
        else if (inputDir.x < 0)
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            render.flipX = true;
            dashRender.flipX = false;
            commonAttackRen.flipX = true;
            attackUpRen.flipX = true;
            jumpAttackDownRen.flipX = true;
            lastDirXCheck = DirX.Left;
        }
    }

    Coroutine lookingRoutine;
    IEnumerator LookingRoutine()
    {
        while (isLook)
        {
            if (playerAttacker.IsAttack())      // 지상에서 위, 아래를 쳐다보던 중 공격을 하면 시점을 재자리로 복귀, 
            {                                   // 공격이 진행 및 끝나도 계속 쳐다보고 있으면(위, 아래 키를 계속 누르고 있으면) 그 방향을 다시 바라보게됨
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;

                yield return new WaitUntil(() => playerAttacker.IsAttack() == false);
            }

            /*if (playerSkiller.IsSkill())      // 지상에서 위, 아래를 쳐다보던 중 공격을 하면 시점을 재자리로 복귀, 
            {                                   // 공격이 진행 및 끝나도 계속 쳐다보고 있으면(위, 아래 키를 계속 누르고 있으면) 그 방향을 다시 바라보게됨
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;

                yield return new WaitUntil(() => playerSkiller.IsSkill() == false);
            }*/

            if (!isGround)                      // 공중에서 위, 아래로 시점이동 방지
            {
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;
                animator.SetFloat("LookUpDown", inputDir.y);
            }

            lookUpDownTime += Time.deltaTime;

            if (inputDir.x != 0 || inputDir.y == 0)     // 위, 아래를 보던 중 이동을 하거나 위, 아래 키를 때면 시점 초기화 
            {
                lookUpDownTime = 0;
                isLook = false;
                animator.SetBool("IsLook", isLook);
                animator.SetFloat("LookUpDown", inputDir.y);
                upDown = UpDown.None;
                isCameraMove = isLook;
                break;
            }
            else if (inputDir.y != 0)
            {
                isLook = true;
                animator.SetBool("IsLook", isLook);
                animator.SetFloat("LookUpDown", inputDir.y);

                if (inputDir.x == 0 && inputDir.y > 0)
                {
                    if (lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Up;
                        isCameraMove = isLook;
                    }
                }
                else if (inputDir.x == 0 && inputDir.y < 0)
                {
                    if (lookUpDownTime > 0.7f)
                    {
                        upDown = UpDown.Down;
                        isCameraMove = isLook;
                    }
                }
            }
            yield return null;
        }
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        
        if (inputDir != new Vector2(0, 0))    // 방향키를 누르지 않고 대시할 때 움직이지 않는거 방지, 대시 중 방향 바뀌는거 방지, 입력이 기존 입력에서 변경되었을 때만
        {
            lookDir = inputDir;

            if (dashDir != lookDir.x && lookDir.x != 0 && !isDash)
            {
                dashDir = lookDir.x;
            }
        }
     
        if (inputDir.y != 0)
        {
            isLook = true;
            lookingRoutine = StartCoroutine(LookingRoutine());
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));
    }

    private void ChangeAttackPositionX()
    {
        if (dirX == DirX.Right)
            commonAttackPoint.Translate(new Vector3(originCommonAttackPointX * 2f, 0));
        else if (dirX == DirX.Left)
            commonAttackPoint.Translate(new Vector3(-originCommonAttackPointX * 2f, 0));
    }

    Coroutine jumpRoutine;
    IEnumerator JumpRoutine()       // 점프 끝나고 내려올 때 
    {
        while (isJump)
        {
            if (limitMove)     // 점프키 누르고있으면 대시가 끝난 후 다시 올라가는 현상 방지
                break;

            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpTime += Time.deltaTime;

            if (jumpTime > 0.4f)
            {
                yield return new WaitForSeconds(0.15f);
                //rb.velocity = new Vector2(rb.velocity.x, -jumpPower);
                break;
            }

            yield return null;
        }

        yield break;
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
            rb.velocity = new Vector2(rb.velocity.x, -jumpPower/2);
        }
    }

    Coroutine dashRoutine;
    IEnumerator DashRoutine()
    {
        limitMove = true;
        isDash = true;
        dashTime = 0f;  // TODO : 대시 애니메이션과 시간 동기화 필요, 점프를 계속 누르고 있으면서 대시를 하면 대시 종료 후 점프하는 현상 수정 필요
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // 대시 중 중력 영향받아 떨어지는거 방지
        animator.SetTrigger("Dash");
        dashAnimator.SetTrigger("OnDashEffect");
        gameObject.layer = LayerMask.NameToLayer("Default");    // 대시 시 무적 구현을 위함

        while (true)
        {
            if (dashDir > 0)
                transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
            else if (dashDir < 0)
                transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));

            dashTime += Time.deltaTime;

            if (dashTime > 0.5f)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                limitMove = false;
                isDash = false;
                dashDir = lookDir.x;
                break;
            }
            yield return null;
        }

        gameObject.layer = LayerMask.NameToLayer("Player");

        yield break;
    }

    private void OnDash(InputValue value)
    {
        if (!limitMove)    // 대시 중 다시 대시하는거 방지
            dashRoutine = StartCoroutine(DashRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 4f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 4f, Color.red);

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

    public float LastDirX()
    {
        return dashDir;
    }

    public bool IsLook()
    {
        return isLook;
    }

    public bool IsCameraMove()
    {
        return isCameraMove;
    }

    public bool IsGround()
    {
        return isGround;
    }

    public bool LimitMove(bool limitMove)
    {
        if (limitMove)  // 중력 영향 안받음, 점프 하자마자 대시하고 점프키를 계속 누르고있어도 대시 끝나면 바로 내려감
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        return this.limitMove = limitMove;
    }
    // rb.simulated는 비용이 많이 듬
    

    public bool LimitMove()
    {
        return limitMove;
    }

    public void StopJumpRoutine()
    {
        StopCoroutine(jumpRoutine);
    }

    /* TODO : isGround가 false일 때 isLook이 바뀌질 않음

    isCameraMove가 쓰이지 않고 있음, CameraController에서 쓸려고 했던 것 같은데 isLook이 바뀌면 CameraMove가 IsLook의 값을 받아서 현재 쓸 이유가 없음
    분명 구분할 이유가 있어서 만들었는데 기억이 안남

    isCameraMove가 isLook이랑 구분될려고 넣은거같음
    AttackUP, JumpAttackDown이 isLook, LookUpDown, Attack을 사용해 진입하기에 

    if(!isGround)가 다 쳐막고있어서?

    그럼 if(!isGround)는 카메라 관련만 건드려야할듯?

    시점 변경이랑 위, 아래 공격 함수를 구분해야 할것같음*/

    // 카메라 의도대로 정상동작함, 수정 완
}
