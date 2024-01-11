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
    private float dashDir;                         // ����Ű �Է� ������ ���ڸ� ����ϴ� ���� ����
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

    private void Move()     // ������ �̵�
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
            if (playerAttacker.IsAttack())      // ���󿡼� ��, �Ʒ��� �Ĵٺ��� �� ������ �ϸ� ������ ���ڸ��� ����, 
            {                                   // ������ ���� �� ������ ��� �Ĵٺ��� ������(��, �Ʒ� Ű�� ��� ������ ������) �� ������ �ٽ� �ٶ󺸰Ե�
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;

                yield return new WaitUntil(() => playerAttacker.IsAttack() == false);
            }

            /*if (playerSkiller.IsSkill())      // ���󿡼� ��, �Ʒ��� �Ĵٺ��� �� ������ �ϸ� ������ ���ڸ��� ����, 
            {                                   // ������ ���� �� ������ ��� �Ĵٺ��� ������(��, �Ʒ� Ű�� ��� ������ ������) �� ������ �ٽ� �ٶ󺸰Ե�
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;

                yield return new WaitUntil(() => playerSkiller.IsSkill() == false);
            }*/

            if (!isGround)                      // ���߿��� ��, �Ʒ��� �����̵� ����
            {
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;
                animator.SetFloat("LookUpDown", inputDir.y);
            }

            lookUpDownTime += Time.deltaTime;

            if (inputDir.x != 0 || inputDir.y == 0)     // ��, �Ʒ��� ���� �� �̵��� �ϰų� ��, �Ʒ� Ű�� ���� ���� �ʱ�ȭ 
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
        
        if (inputDir != new Vector2(0, 0))    // ����Ű�� ������ �ʰ� ����� �� �������� �ʴ°� ����, ��� �� ���� �ٲ�°� ����, �Է��� ���� �Է¿��� ����Ǿ��� ����
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
    IEnumerator JumpRoutine()       // ���� ������ ������ �� 
    {
        while (isJump)
        {
            if (limitMove)     // ����Ű ������������ ��ð� ���� �� �ٽ� �ö󰡴� ���� ����
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
        dashTime = 0f;  // TODO : ��� �ִϸ��̼ǰ� �ð� ����ȭ �ʿ�, ������ ��� ������ �����鼭 ��ø� �ϸ� ��� ���� �� �����ϴ� ���� ���� �ʿ�
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; // ��� �� �߷� ����޾� �������°� ����
        animator.SetTrigger("Dash");
        dashAnimator.SetTrigger("OnDashEffect");
        gameObject.layer = LayerMask.NameToLayer("Default");    // ��� �� ���� ������ ����

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
        if (!limitMove)    // ��� �� �ٽ� ����ϴ°� ����
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

    // ���� �Լ����� �ٸ� ������Ʈ��� ��ȣ�ۿ�

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
        if (limitMove)  // �߷� ���� �ȹ���, ���� ���ڸ��� ����ϰ� ����Ű�� ��� �������־ ��� ������ �ٷ� ������
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        return this.limitMove = limitMove;
    }
    // rb.simulated�� ����� ���� ��
    

    public bool LimitMove()
    {
        return limitMove;
    }

    public void StopJumpRoutine()
    {
        StopCoroutine(jumpRoutine);
    }

    /* TODO : isGround�� false�� �� isLook�� �ٲ��� ����

    isCameraMove�� ������ �ʰ� ����, CameraController���� ������ �ߴ� �� ������ isLook�� �ٲ�� CameraMove�� IsLook�� ���� �޾Ƽ� ���� �� ������ ����
    �и� ������ ������ �־ ������µ� ����� �ȳ�

    isCameraMove�� isLook�̶� ���еɷ��� �����Ű���
    AttackUP, JumpAttackDown�� isLook, LookUpDown, Attack�� ����� �����ϱ⿡ 

    if(!isGround)�� �� �ĸ����־?

    �׷� if(!isGround)�� ī�޶� ���ø� �ǵ�����ҵ�?

    ���� �����̶� ��, �Ʒ� ���� �Լ��� �����ؾ� �ҰͰ���*/

    // ī�޶� �ǵ���� ��������, ���� ��
}
