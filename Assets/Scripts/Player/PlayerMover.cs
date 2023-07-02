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
    private float dashDir;                         // ����Ű �Է� ������ ���ڸ� ����ϴ� ���� ����
    private float lookUpDownTime;
    private float jumpTime;
    private float dashTime;
    private bool limitMove;
    private bool isDash;
    private bool isLook;
    private bool isJump;
    private bool isGround;
    private bool isCameraMove;
    private UpDown upDown;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        dashDir = lookDir.x;
    }

    private void Update()
    {
        if (!limitMove) 
            Move();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void Move()     // ������ �̵�
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
            if (playerAttacker.IsAttack())      // ���󿡼� ��, �Ʒ��� �Ĵٺ��� �� ������ �ϸ� ������ ���ڸ��� ����, 
            {                                   // ������ ���� �� ������ ��� �Ĵٺ��� ������(��, �Ʒ� Ű�� ��� ������ ������) �� ������ �ٽ� �ٶ󺸰Ե�
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;

                yield return new WaitUntil(() => playerAttacker.IsAttack() == false);
            }

            //if (playerAttacker.IsSkill())      // ���󿡼� ��, �Ʒ��� �Ĵٺ��� �� ������ �ϸ� ������ ���ڸ��� ����, 
            //{                                   // ������ ���� �� ������ ��� �Ĵٺ��� ������(��, �Ʒ� Ű�� ��� ������ ������) �� ������ �ٽ� �ٶ󺸰Ե�
            //    lookUpDownTime = 0;
            //    upDown = UpDown.None;
            //    isCameraMove = false;

            //    yield return new WaitUntil(() => playerAttacker.IsSkill() == false);
            //}

            if (!isGround)                      // ���߿��� ��, �Ʒ��� �����̵� ����
            {
                lookUpDownTime = 0;
                upDown = UpDown.None;
                isCameraMove = false;
                animator.SetFloat("LookUpDown", inputDir.y);

                //yield return new WaitUntil(() => isGround == true);
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
            else if(inputDir.y != 0)
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
        
        if(inputDir != new Vector2(0, 0) && !isDash)    // ����Ű�� ������ �ʰ� ����� �� �������� �ʴ°� ����, ��� �� ���� �ٲ�°� ����, �Է��� ���� �Է¿��� ����Ǿ��� ����
        {
            lookDir = value.Get<Vector2>();

            if (dashDir != lookDir.x && lookDir.x != 0)
                dashDir = lookDir.x;
        }

        if(inputDir.y != 0)
        {
            isLook = true;
            lookingRoutine = StartCoroutine(LookingRoutine());
        }

        animator.SetFloat("Move", Mathf.Abs(inputDir.x));
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

            if (!isGround)
                rb.velocity = new Vector2(rb.velocity.x, -jumpPower);
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

        while (true)
        {
            if (dashDir > 0)
                transform.Translate(new Vector2(dashSpeed * Time.deltaTime, 0));
            else if (dashDir < 0)
                transform.Translate(new Vector2(-dashSpeed * Time.deltaTime, 0));

            dashTime += Time.deltaTime;

            if(dashTime > 0.5f)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                limitMove = false;
                isDash = false;
                //LookSync();
                break;
            }
            yield return null;
        }
    }

    private void OnDash(InputValue value)
    {
        if (!limitMove)    // ��� �� �ٽ� ����ϴ°� ����
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

    // ���� �Լ����� �ٸ� ������Ʈ��� ��ȣ�ۿ�

    //public void LookSync()
    //{
    //    Debug.Log(isLook);
    //    Debug.Log(inputDir.y);
    //    animator.SetBool("IsLook", isLook);
    //    animator.SetFloat("LookUpDown", inputDir.y);
    //    dashDir = inputDir.x;      
    //}

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

    public bool LimitMove(bool limitMove)   // �Է°��� ���� ���ϰ�
    {
        if (limitMove)
            rb.simulated = false;           // �߷� ���� �ȹ���
        else
            rb.simulated = true;

        return this.limitMove = limitMove;
    }

    public bool LimitMove()
    {
        return limitMove;
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
